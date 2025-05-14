using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services;

public class FlowService(IFlowRepository flowRepository, ILogger<FlowService> logger) : IFlowService
{
  private readonly IFlowRepository _flowRepository = flowRepository;
  private readonly ILogger<FlowService> _logger = logger;

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

}