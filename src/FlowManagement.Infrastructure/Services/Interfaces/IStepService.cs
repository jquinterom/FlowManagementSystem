using FlowManagement.Core.Entities;

namespace FlowManagement.Infrastructure.Services.Interfaces
{
  public interface IStepService
  {
    Task<List<Step>> GetStepsAsync();
    Task CreateStepAsync(Step step);
  }
}