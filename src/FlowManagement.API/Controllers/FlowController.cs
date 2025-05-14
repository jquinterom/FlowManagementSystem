using FlowManagement.API.Models;
using FlowManagement.Core.Entities;
using FlowManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/flows")]
public class FlowController(IFlowService flowService, ILogger<FlowController> logger) : ControllerBase
{
  private readonly IFlowService _flowService = flowService;
  private readonly ILogger<FlowController> _logger = logger;

  [HttpGet]
  public async Task<IActionResult> GetFlows()
  {
    var flows = await _flowService.GetFlowAsync();

    _logger.LogInformation($"Flow list: {flows.Count}");

    return (flows != null) ? Ok(flows) : NotFound();
  }


  [HttpGet("{flowId}")]
  public async Task<IActionResult> GetFlowById(Guid flowId)
  {
    var flow = await _flowService.GetFlowByIdAsync(flowId);

    _logger.LogInformation($"Flow with steps list: {flow.Steps.Count}");

    return (flow != null) ? Ok(flow) : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> CreateFlow([FromBody] CreateFlowDto dto)
  {
    var flow = new Flow
    {
      Id = Guid.NewGuid(),
      Name = dto.Name,
      Purpose = dto.Purpose,
      Description = dto.Description ?? string.Empty,
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };

    await _flowService.CreateFlowAsync(flow);

    return CreatedAtAction(nameof(GetFlows), new { flow.Id }, flow);
  }

  [HttpPost("{flowId}/steps")]
  [ProducesResponseType(typeof(Step), 201)]
  public async Task<IActionResult> AddStepToFlow(Guid flowId, [FromBody] CreateStepDto stepDto)
  {
    var step = new Step
    {
      Code = stepDto.Code,
      Name = stepDto.Name,
      Description = stepDto.Description ?? string.Empty,
      Order = stepDto.Order,
      IsParallel = stepDto.IsParallel,
      ActionType = stepDto.ActionType,
      CreatedAt = DateTime.UtcNow
    };

    var createdStep = await _flowService.AddStepToFlowAsync(flowId, step);

    return CreatedAtAction(
        nameof(GetFlowById),
        new { flowId },
        createdStep);
  }
}