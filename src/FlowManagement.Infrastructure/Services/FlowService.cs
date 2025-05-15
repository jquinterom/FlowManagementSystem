using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services
{
  public class FlowService(IFlowRepository flowRepository, ILogger<FlowService> logger, IStepRepository stepRepository) : IFlowService
  {
    private readonly IFlowRepository _flowRepository = flowRepository;
    private readonly ILogger<FlowService> _logger = logger;
    private readonly IStepRepository _stepRepository = stepRepository;

    public async Task<List<Flow>> GetFlowAsync()
    {
      var flows = await _flowRepository.GetAllAsync();
      return [.. flows];
    }

    public async Task CreateFlowAsync(Flow flow)
    {
      await _flowRepository.AddAsync(flow);
      _logger.LogInformation($"Flow created with id {flow.Id}");
    }

    public async Task<IEnumerable<Step>> AddStepsToFlowAsync(Guid flowId, string[] stepCodes)
    {
      IEnumerable<Step>? steps = await _stepRepository.GetStepsByCodeAsync(stepCodes) ?? throw new InvalidOperationException("The steps do not exist");

      var notExistsSteps = ValidateStepsDoesNotExists(stepCodes, [.. steps]);

      if (notExistsSteps != null)
      {
        throw new InvalidOperationException($"The following steps do not exist: {string.Join(", ", notExistsSteps)}");
      }

      // Exists steps by flow
      var existingStepsByFlowId = await _flowRepository.GetStepsByFlowAsync(flowId);

      if (existingStepsByFlowId != null)
      {
        foreach (var step in existingStepsByFlowId)
        {
          if (steps.Any(s => s.Code == step))
          {
            throw new InvalidOperationException($"This step already exists: {step}");
          }
        }
      }

      foreach (var step in stepCodes)
      {
        await _flowRepository.AddStepToFlowAsync(flowId, step);
      }

      return steps;
    }

    public async Task<Flow> GetFlowByIdAsync(Guid flowId)
    {
      var flow = await _flowRepository.GetByIdAsync(flowId);

      return flow ?? throw new Exception("Flow not found");
    }

    public static string[]? ValidateStepsDoesNotExists(string[] stepCodes, Step[] steps)
    {
      List<string> notExistsSteps = [];

      foreach (var stepCode in stepCodes)
      {
        var step = steps.FirstOrDefault(s => s.Code == stepCode);
        if (step == null)
        {
          notExistsSteps.Add(stepCode);
        }
      }

      if (notExistsSteps.Count != 0)
      {
        return [.. notExistsSteps];
      }

      return null;
    }
  }
}