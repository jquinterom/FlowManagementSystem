using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Data;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class FlowExecutionRepository(MongoDbContext context) : BaseRepository<FlowExecution>(context.FlowExecutions), IFlowExecutionRepository
{
  public async Task<FlowExecution> StartExecutionAsync(Guid flowId)
  {
    var execution = new FlowExecution { FlowId = flowId };
    await _collection.InsertOneAsync(execution);
    return execution;
  }

  public async Task UpdateExecutionStatusAsync(Guid executionId, ExecutionStatus status)
  {
    var filter = Builders<FlowExecution>.Filter.Eq(e => e.ExecutionId, executionId);
    var update = Builders<FlowExecution>.Update
        .Set(e => e.Status, status)
        .Set(e => e.EndTime, status != ExecutionStatus.Running ? DateTime.UtcNow : null);

    await _collection.UpdateOneAsync(filter, update);
  }

  public async Task UpdateExecutionAsync(Guid executionId, UpdateDefinition<FlowExecution> update)
  {
    var filter = Builders<FlowExecution>.Filter.Eq(e => e.ExecutionId, executionId);
    await _collection.UpdateOneAsync(filter, update);
  }

  public async Task AddStepExecutionAsync(Guid executionId, StepExecution stepExecution)
  {
    var filter = Builders<FlowExecution>.Filter.Eq(e => e.ExecutionId, executionId);
    var update = Builders<FlowExecution>.Update.Push(e => e.StepExecutions, stepExecution);

    await _collection.UpdateOneAsync(filter, update);
  }
}