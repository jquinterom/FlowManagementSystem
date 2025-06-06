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
    IStepRepository stepRepository,
    ILogger<FlowExecutionService> logger,
    IFlowExecutionRepository executionRepository) : IFlowExecutionService
{
  private readonly IFlowRepository _flowRepository = flowRepository;
  private readonly IFieldRepository _fieldRepository = fieldRepository;
  private readonly IStepRepository _stepRepository = stepRepository;
  private readonly ILogger<FlowExecutionService> _logger = logger;
  private readonly IFlowExecutionRepository _executionRepository = executionRepository;

  public async Task<Guid> ExecuteFlowAsync(Guid flowId, Dictionary<string, object> inputs)
  {
    await ValidateInputFieldsAsync(inputs.Keys);

    var execution = await _executionRepository.StartExecutionAsync(flowId);
    var context = new ExecutionContext(execution.ExecutionId) { CurrentInputs = inputs };

    try
    {
      var flow = await _flowRepository.GetByIdAsync(flowId);
      var stepCodes = flow.Steps.ToArray();

      IEnumerable<Step>? steps = await _stepRepository.GetStepsByCodeAsync(stepCodes) ?? throw new InvalidOperationException($"The steps do not exist in the flow {flowId}");

      var stepsGrouped = steps.GroupBy(s => s.Order).OrderBy(g => g.Key);

      foreach (var group in stepsGrouped)
      {
        if (group.Any(stepGroup => stepGroup.IsParallel))
        {
          var parallelTasks = group
              .Where(stepGroup => stepGroup.IsParallel)
              .Select(stepGroup => ExecuteStepWithTrackingAsync(stepGroup, context));
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
          execution.Id,
          Builders<FlowExecution>.Update
              .Set(e => e.Status, ExecutionStatus.Completed)
              .Set(e => e.EndTime, DateTime.UtcNow)
              .Set(e => e.FinalOutputs, context.CollectedOutputs));

      _logger.LogInformation("Execution completed: {ExecutionId}", execution.Status.ToString());
      return execution.Id;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error executing flow {FlowId}", flowId);
      await _executionRepository.UpdateExecutionStatusAsync(
          execution.Id,
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
        .Where(i => i.IsRequired && !context.CurrentInputs.ContainsKey(i.FieldCode))
        .ToList();


    if (missingInputs.Count != 0)
    {
      throw new InvalidOperationException(
          $"It is missing required inputs: {string.Join(", ", missingInputs.Select(i => i.FieldCode))}");
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
            $"Dependency not satisfied: required '{dependency.RequiredOutput}' from step {dependency.StepCode}");
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

  private async Task ValidateInputFieldsAsync(IEnumerable<string> fieldCodes)
  {
    var invalidFields = new List<string>();

    foreach (var code in fieldCodes)
    {
      var fieldExists = await _fieldRepository.AnyAsync(f => f.Code == code);
      if (!fieldExists)
      {
        invalidFields.Add(code);
      }
    }

    if (invalidFields.Count != 0)
    {
      throw new InvalidOperationException(
          $"The following fields are not registered: {string.Join(", ", invalidFields)}. " +
          "Please register them before executing the flow.");
    }
  }

  public async Task<FlowExecution> GetExecutionByIdAsync(Guid executionId)
  {
    var execution = await _executionRepository.GetByIdAsync(executionId);
    _logger.LogInformation("Execution found: {ExecutionId}", execution?.Status);
    return execution ?? throw new KeyNotFoundException("Execution not found");
  }
}

public class ExecutionContext(Guid executionId)
{
  public Guid ExecutionId { get; set; } = executionId;
  public Dictionary<string, object> CurrentInputs { get; set; } = [];
  public Dictionary<string, object> CollectedOutputs { get; set; } = [];
}