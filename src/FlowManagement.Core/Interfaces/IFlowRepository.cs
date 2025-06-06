using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IFlowRepository : IRepository<Flow>
{
  Task<IEnumerable<string>> GetStepsByFlowAsync(Guid flowId);
  Task AddStepToFlowAsync(Guid flowId, string step);
  Task<bool> FlowExistsAsync(Guid flowId);
  Task AddFlowAsync(Flow flow);
}