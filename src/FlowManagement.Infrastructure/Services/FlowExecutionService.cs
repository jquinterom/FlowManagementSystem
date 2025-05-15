using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Repositories;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Services;

public class FlowExecutionService(
    IFlowRepository flowRepository,
    IFieldRepository fieldRepository,
    ILogger<FlowExecutionService> logger,
    IFlowExecutionRepository executionRepository) : IFlowExecutionService
{
  private readonly IFlowRepository _flowRepository = flowRepository;
  private readonly IFieldRepository _fieldRepository = fieldRepository;
  private readonly ILogger<FlowExecutionService> _logger = logger;
  private readonly IFlowExecutionRepository _executionRepository = executionRepository;

  public async Task<Guid> ExecuteFlowAsync(Guid flowId, Dictionary<string, object> inputs)
  {
    var execution = await _executionRepository.StartExecutionAsync(flowId);
    var context = new ExecutionContext(execution.ExecutionId) { CurrentInputs = inputs };

    try
    {
      var flow = await _flowRepository.GetByIdAsync(flowId);
      var stepsGrouped = flow.Steps.GroupBy(s => s.Order).OrderBy(g => g.Key);

      foreach (var group in stepsGrouped)
      {
        if (group.Any(s => s.IsParallel))
        {
          var parallelTasks = group
              .Where(s => s.IsParallel)
              .Select(s => ExecuteStepWithTrackingAsync(s, context));
          await Task.WhenAll(parallelTasks);
        }
        else
        {
          foreach (var step in group)
          {
            await ExecuteStepWithTrackingAsync(step, context);
          }
        }
      }

      await _executionRepository.UpdateExecutionAsync(
          execution.ExecutionId,
          Builders<FlowExecution>.Update
              .Set(e => e.Status, ExecutionStatus.Completed)
              .Set(e => e.EndTime, DateTime.UtcNow)
              .Set(e => e.FinalOutputs, context.CollectedOutputs));

      return execution.ExecutionId;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error executing flow {FlowId}", flowId);
      await _executionRepository.UpdateExecutionStatusAsync(
          execution.ExecutionId,
          ExecutionStatus.Failed);
      throw;
    }
  }

  private async Task ExecuteStepWithTrackingAsync(Step step, ExecutionContext context)
  {
    var stepExecution = new StepExecution { StepId = step.Id, ExecutionTime = DateTime.UtcNow, Status = ExecutionStatus.Running };

    try
    {
      _logger.LogInformation("Executing step {StepName} (Order: {Order})", step.Name, step.Order);

      // 1. Validar dependencias
      ValidateDependencies(step, context);

      // 2. Ejecutar paso
      var outputs = ExecuteStep(step, context);

      // 3. Registrar resultados
      stepExecution.Status = ExecutionStatus.Completed;
      stepExecution.Outputs = await outputs;
    }
    catch (Exception ex)
    {
      stepExecution.Status = ExecutionStatus.Failed;
      stepExecution.Error = ex.Message;
      throw;
    }
    finally
    {
      await _executionRepository.AddStepExecutionAsync(context.ExecutionId, stepExecution);
    }
  }


  private async Task<Dictionary<string, object>> ExecuteStep(Step step, ExecutionContext context)
  {
    _logger.LogInformation("Executing step {StepName} (Order: {Order})", step.Name, step.Order);

    // 1. Validar inputs requeridos
    var missingInputs = step.Inputs
        .Where(i => i.IsRequired && !context.CurrentInputs.ContainsKey(i.Field.Code))
        .ToList();

    if (missingInputs.Count != 0)
    {
      throw new InvalidOperationException(
          $"It is missing required inputs: {string.Join(", ", missingInputs.Select(i => i.Field.Code))}");
    }

    // 2. Ejecutar lógica del paso (ejemplo simplificado)
    var stepOutputs = new Dictionary<string, object>();
    switch (step.ActionType)
    {
      case ActionType.DocumentValidation:
        stepOutputs = ValidateDocuments(context.CurrentInputs);
        break;
      case ActionType.EmailNotification:
        stepOutputs = await SendEmailAsync(context.CurrentInputs);
        break;
        // ... otros tipos de acción`
    }

    // 3. Guardar outputs
    foreach (var output in stepOutputs)
    {
      context.CollectedOutputs[output.Key] = output.Value;
    }

    return stepOutputs;
  }

  // Ejemplo de método para un tipo de paso
  private static Dictionary<string, object> ValidateDocuments(Dictionary<string, object> inputs)
  {
    // Lógica de validación...
    return new Dictionary<string, object>
    {
      ["validationStatus"] = "approved",
      ["validationDate"] = DateTime.UtcNow
    };
  }

  public static void ValidateDependencies(Step step, ExecutionContext context)
  {
    foreach (var dependency in step.Dependencies)
    {
      if (!context.CollectedOutputs.ContainsKey(dependency.RequiredOutput))
      {
        throw new InvalidOperationException(
            $"Dependencia no satisfecha: se requiere '{dependency.RequiredOutput}' del paso {dependency.StepId}");
      }
    }
  }

  private async Task<Dictionary<string, object>> SendEmailAsync(Dictionary<string, object> inputs)
  {
    var email = inputs["email"].ToString();
    var body = inputs["body"].ToString();

    // Simulación de envío
    await Task.Delay(500);
    _logger.LogInformation("Email enviado a {Email}", email);

    return new Dictionary<string, object>
    {
      ["emailStatus"] = "delivered",
      ["sentAt"] = DateTime.UtcNow
    };
  }
}

public class ExecutionContext(Guid executionId)
{
  public Guid ExecutionId { get; set; } = executionId;
  public Dictionary<string, object> CurrentInputs { get; set; } = [];
  public Dictionary<string, object> CollectedOutputs { get; set; } = [];
}