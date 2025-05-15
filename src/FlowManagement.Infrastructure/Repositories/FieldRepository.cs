using System.Linq.Expressions;
using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Data;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class FieldRepository(MongoDbContext context) : BaseRepository<Field>(context.Fields), IFieldRepository
{
  private readonly IMongoCollection<Field> _fieldCollection = context.Fields;

  public async Task<Field> GetByCodeAsync(string code)
  {
    return await _fieldCollection.Find(f => f.Code == code).FirstOrDefaultAsync();
  }

  public async Task<bool> AnyAsync(Expression<Func<Field, bool>> predicate)
  {
    return await _fieldCollection.Find(predicate).AnyAsync();
  }
}