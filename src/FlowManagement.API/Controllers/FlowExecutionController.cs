using FlowManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/flows/{flowId}/execute")]
public class FlowExecutionController(FlowExecutionService executionService, ILogger<FlowExecutionController> logger) : ControllerBase
{
  private readonly FlowExecutionService _executionService = executionService;
  private readonly ILogger _logger = logger;

  [HttpPost]
  [ProducesResponseType(typeof(ExecutionResponse), 200)]
  [ProducesResponseType(400)]
  [ProducesResponseType(404)]
  public async Task<IActionResult> ExecuteFlow(Guid flowId, [FromBody] ExecuteFlowRequest request)
  {
    try
    {
      var executionId = await _executionService.ExecuteFlowAsync(flowId, request.Inputs);
      return Ok(new ExecutionResponse(executionId, "Execution started successfully"));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex, "Flow not found: {FlowId}", flowId);
      return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex, "Error validating flow: {FlowId}", flowId);
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error executing flow: {FlowId}", flowId);
      return StatusCode(500, "Server error");
    }
  }
}
