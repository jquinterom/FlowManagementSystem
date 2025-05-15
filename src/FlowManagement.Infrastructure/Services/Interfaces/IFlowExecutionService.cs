namespace FlowManagement.Infrastructure.Services.Interfaces;

public interface IFlowExecutionService
{
  Task<Guid> ExecuteFlowAsync(Guid flowId, Dictionary<string, object> inputs);

}