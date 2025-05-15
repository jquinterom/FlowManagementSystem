using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services
{
  public class StepService(IStepRepository stepRepository, ILogger<StepService> logger) : IStepService
  {
    private readonly IStepRepository _stepRepository = stepRepository;
    private readonly ILogger<StepService> _logger = logger;

    public async Task CreateStepAsync(Step step)
    {
      await _stepRepository.AddAsync(step);
      _logger.LogInformation($"Step created with id {step.Id}");
    }


    public async Task<List<Step>> GetStepsAsync()
    {
      var steps = await _stepRepository.GetAllAsync();
      return [.. steps];
    }
  }
}