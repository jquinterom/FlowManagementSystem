using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public interface IFlowExecutionRepository : IRepository<FlowExecution>
{
  Task<FlowExecution> StartExecutionAsync(Guid flowId);
  Task UpdateExecutionStatusAsync(Guid executionId, ExecutionStatus status);
  Task AddStepExecutionAsync(Guid executionId, StepExecution stepExecution);
  Task UpdateExecutionAsync(Guid executionId, UpdateDefinition<FlowExecution> update);
}