using FlowManagement.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Data;

public class MongoDbContext
{
  private readonly IMongoDatabase _database;

  public MongoDbContext(IOptions<MongoDbSettings> settings)
  {
    var client = new MongoClient(settings.Value.ConnectionString);
    _database = client.GetDatabase(settings.Value.DatabaseName);
  }

  public IMongoCollection<Flow> Flows => _database.GetCollection<Flow>("flows");
  public IMongoCollection<Step> Steps => _database.GetCollection<Step>("steps");
  public IMongoCollection<Field> Fields => _database.GetCollection<Field>("fields");
  public IMongoCollection<StepInput> StepInputs => _database.GetCollection<StepInput>("stepInputs");
  public IMongoCollection<StepOutput> StepOutputs => _database.GetCollection<StepOutput>("stepOutputs");
  public IMongoCollection<FlowExecution> FlowExecutions => _database.GetCollection<FlowExecution>("flowExecutions");
}