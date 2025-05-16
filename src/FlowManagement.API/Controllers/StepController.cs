using FlowManagement.Core.Entities;
using FlowManagement.Core.Models;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/steps")]
public class StepController(IStepService stepService, ILogger<StepController> logger) : ControllerBase
{
  private readonly IStepService _stepService = stepService;
  private readonly ILogger<StepController> _logger = logger;


  [HttpGet]
  public async Task<IActionResult> GetSteps()
  {
    var steps = await _stepService.GetStepsAsync();

    _logger.LogInformation($"Steps list: {steps.Count}");

    return (steps != null) ? Ok(steps) : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> CreateStep([FromBody] CreateStepDto dto)
  {
    try
    {
      Step step = await _stepService.CreateStepAsync(dto);

      return CreatedAtAction(nameof(GetSteps), new { step.Id }, step);
    }
    catch (KeyNotFoundException ex)
    {
      return BadRequest(ex.Message);
    }
  }
}