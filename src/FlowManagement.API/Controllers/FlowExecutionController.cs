using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/flows/execute/")]
public class FlowExecutionController(IFlowExecutionService executionService, ILogger<FlowExecutionController> logger) : ControllerBase
{
  private readonly IFlowExecutionService _executionService = executionService;
  private readonly ILogger _logger = logger;

  [HttpPost("{flowId}")]
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
    catch (InvalidOperationException ex) when (ex.Message.Contains("not registered"))
    {
      return BadRequest(new
      {
        Error = "Fields not registered",
        Details = ex.Message,
        Solution = "Register the fields using the api POST /api/fields"
      });
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

  [HttpGet("{executionId}")]
  [ProducesResponseType(typeof(ExecutionResponse), 200)]
  public async Task<IActionResult> GetExecution(Guid executionId)
  {
    try
    {
      var execution = await _executionService.GetExecutionByIdAsync(executionId);
      return Ok(new ExecutionResponse(execution.ExecutionId, execution.Status.ToString()));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex, "Execution not found: {ExecutionId}", executionId);
      return NotFound(ex.Message);
    }
  }
}
