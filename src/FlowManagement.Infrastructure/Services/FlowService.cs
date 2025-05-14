using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services;


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

  public async Task<Step> AddStepToFlowAsync(Guid flowId, Step step)
  {
    // Validación de orden duplicado
    var existingSteps = await _flowRepository.GetStepsByFlowAsync(flowId);
    if (existingSteps.Any(s => s.Order == step.Order))
    {
      throw new InvalidOperationException($"Ya existe un paso con orden {step.Order}");
    }

    await _flowRepository.AddStepToFlowAsync(flowId, step);

    return step;
  }


  public async Task<Flow> GetFlowByIdAsync(Guid flowId)
  {
    var flow = await _flowRepository.GetByIdAsync(flowId);
    if (flow != null)
    {
      flow.Steps = [.. flow.Steps.OrderBy(s => s.Order)];
    }

    return flow ?? throw new Exception("Flow not found");
  }
}