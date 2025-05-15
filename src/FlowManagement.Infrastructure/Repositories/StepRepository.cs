using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Data;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class StepRepository(MongoDbContext context) : BaseRepository<Step>(context.Steps), IStepRepository
{
  private readonly IMongoCollection<Step> _stepCollection = context.Steps;

  public Task CreateStepAsync(Step step)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Step>?> GetStepsByCodeAsync(string[] stepCodes)
  {
    var filter = Builders<Step>.Filter.In(s => s.Code, stepCodes);
    var steps = await _stepCollection.Find(filter).ToListAsync();

    if (steps == null) return null;
    return [.. steps];
  }

  public Task<List<Step>> GetStepsAsync()
  {
    throw new NotImplementedException();
  }
}