using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IFlowRepository : IRepository<Flow>
{
  Task<Flow> GetFlowWithStepsAsync(Guid flowId);
  Task AddStepToFlowAsync(Guid flowId, Step step);
  Task<bool> FlowExistsAsync(Guid flowId);
  Task AddFlowAsync(Flow flow);
}