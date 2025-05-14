using FlowManagement.API.Models;
using FlowManagement.Core.Entities;
using FlowManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/flows")]
public class FlowController : ControllerBase
{
  private readonly IFlowService _flowService;
  private readonly ILogger<FlowController> _logger;

  public FlowController(IFlowService flowService, ILogger<FlowController> logger)
  {
    _flowService = flowService;
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetFlows()
  {
    var flows = await _flowService.GetFlowAsync();

    _logger.LogInformation($"Flow list: {flows.Count}");

    return (flows != null) ? Ok(flows) : NotFound();
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

}