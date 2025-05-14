using FlowManagement.Core.Entities;

namespace FlowManagement.Infrastructure.Services;

public interface IStepService
{
  Task<List<Step>> GetStepsAsync();
  Task CreateStepAsync(Step step);
}