using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IStepRepository : IRepository<Step>
{
  Task<List<Step>> GetStepsAsync();
  Task CreateStepAsync(Step step);
  Task<IEnumerable<Step>?> GetStepsByCodeAsync(string[] stepCodes);
}