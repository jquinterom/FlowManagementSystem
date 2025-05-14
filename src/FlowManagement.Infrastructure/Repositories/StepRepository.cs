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

  public Task<List<Step>> GetStepsAsync()
  {
    throw new NotImplementedException();
  }
}