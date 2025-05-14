using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IFlowRepository : IRepository<Flow>
{
  Task<IEnumerable<Step>> GetStepsByFlowAsync(Guid flowId);
  Task AddStepToFlowAsync(Guid flowId, Step step);
  Task<bool> FlowExistsAsync(Guid flowId);
  Task AddFlowAsync(Flow flow);
}