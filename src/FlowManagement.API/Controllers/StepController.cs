using FlowManagement.API.Models;
using FlowManagement.Core.Entities;
using FlowManagement.Infrastructure.Services;
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
    var step = new Step
    {
      Id = Guid.NewGuid(),
      Code = dto.Code,
      Name = dto.Name,
      Description = dto.Description ?? string.Empty,
      Order = dto.Order,
      IsParallel = dto.IsParallel,
      ActionType = dto.ActionType
    };

    await _stepService.CreateStepAsync(step);

    return CreatedAtAction(nameof(GetSteps), new { step.Id }, step);
  }
}