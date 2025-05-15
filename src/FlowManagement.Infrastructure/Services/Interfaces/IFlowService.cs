using FlowManagement.Core.Entities;

namespace FlowManagement.Infrastructure.Services.Interfaces
{
  public interface IFlowService
  {
    Task<List<Flow>> GetFlowAsync();
    Task CreateFlowAsync(Flow flow);
    Task<Flow> GetFlowByIdAsync(Guid flowId);
    Task<IEnumerable<Step>> AddStepsToFlowAsync(Guid flowId, string[] stepCodes);
  }
}