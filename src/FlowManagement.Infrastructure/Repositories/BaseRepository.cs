using FlowManagement.Core.Interfaces;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public abstract class BaseRepository<T>(IMongoCollection<T> collection) : IRepository<T> where T : class
{
  protected readonly IMongoCollection<T> _collection = collection;

  public async Task<T> GetByIdAsync(Guid id)
  {
    var filter = Builders<T>.Filter.Eq("Id", id);
    return await _collection.Find(filter).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _collection.Find(_ => true).ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    await _collection.InsertOneAsync(entity);
  }

  public async Task UpdateAsync(T entity)
  {
    var filter = Builders<T>.Filter.Eq("Id", (entity as dynamic).Id);
    await _collection.ReplaceOneAsync(filter, entity);
  }

  public async Task DeleteAsync(T entity)
  {
    var filter = Builders<T>.Filter.Eq("Id", (entity as dynamic).Id);
    await _collection.DeleteOneAsync(filter);
  }
}