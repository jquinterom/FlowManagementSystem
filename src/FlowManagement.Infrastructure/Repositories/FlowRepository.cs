using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Data;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class FlowRepository(MongoDbContext context) : BaseRepository<Flow>(context.Flows), IFlowRepository
{
  private readonly IMongoCollection<Flow> _flowCollection = context.Flows;
  private readonly IMongoCollection<Step> _stepCollection = context.Steps;

  public async Task<IEnumerable<string>> GetStepsByFlowAsync(Guid flowId)
  {
    var flow = await _collection
        .Find(f => f.Id == flowId)
        .FirstOrDefaultAsync();

    return flow?.Steps ?? Enumerable.Empty<string>();
  }

  public async Task AddStepToFlowAsync(Guid flowId, string step)
  {
    var filter = Builders<Flow>.Filter.Eq(f => f.Id, flowId);
    var update = Builders<Flow>.Update.Push(f => f.Steps, step);

    await _collection.UpdateOneAsync(filter, update);
  }

  public async Task<bool> FlowExistsAsync(Guid flowId)
  {
    return await _flowCollection.CountDocumentsAsync(f => f.Id == flowId) > 0;
  }

  public async Task AddFlowAsync(Flow flow)
  {
    await _flowCollection.InsertOneAsync(flow);
  }
}