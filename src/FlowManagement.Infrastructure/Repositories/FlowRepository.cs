using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Data;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class FlowRepository : BaseRepository<Flow>, IFlowRepository
{
  private readonly IMongoCollection<Flow> _flowCollection;
  private readonly IMongoCollection<Step> _stepCollection;

  public FlowRepository(MongoDbContext context)
      : base(context.Flows)
  {
    _flowCollection = context.Flows;
    _stepCollection = context.Steps;
  }

  public async Task<Flow> GetFlowWithStepsAsync(Guid flowId)
  {
    var flow = await _flowCollection.Find(f => f.Id == flowId).FirstOrDefaultAsync();
    if (flow != null)
    {
      flow.Steps = await _stepCollection.Find(s => s.FlowId == flowId)
                                      .ToListAsync();
    }
    return flow ?? throw new Exception("Flow not found");
  }

  public async Task AddStepToFlowAsync(Guid flowId, Step step)
  {
    step.FlowId = flowId;
    await _stepCollection.InsertOneAsync(step);
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