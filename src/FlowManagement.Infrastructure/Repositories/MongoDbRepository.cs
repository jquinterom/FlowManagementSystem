using FlowManagement.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Repositories;

public class MongoDbRepository<T> where T : class
{
  private readonly IMongoCollection<T>? _collection;

  public MongoDbRepository(IOptions<MongoDbSettings> settings, IMongoClient mongoClient)
  {
    if (settings != null && mongoClient != null)
    {
      var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
      _collection = database.GetCollection<T>(settings.Value.CollectionName);
    }
  }

  public async Task InsertAsync(T entity)
  {
    if (_collection == null) return;
    await _collection.InsertOneAsync(entity);
  }

  public async Task<List<T>> GetAllAsync()
  {
    if (_collection == null) return [];
    return await _collection.Find(_ => true).ToListAsync();
  }

  public async Task<T?> GetByIdAsync(T entity)
  {
    ArgumentNullException.ThrowIfNull(entity);

    var idValue = GetIdPropertyValue(entity);

    var filter = Builders<T>.Filter.Eq("Id", idValue);

    var result = await _collection.Find(filter).FirstOrDefaultAsync();

    return result;
  }


  public async Task<bool> DeleteOneAsync(T entity)
  {
    ArgumentNullException.ThrowIfNull(entity);

    var idValue = GetIdPropertyValue(entity);

    var filter = Builders<T>.Filter.Eq("Id", idValue);

    try
    {
      if (_collection == null) return false;

      var result = await _collection.DeleteOneAsync(filter);
      return result.DeletedCount > 0;
    }
    catch (MongoException ex)
    {
      throw new Exception("Error to deleting entity from MongoDB", ex);
    }
  }




  private static object? GetIdPropertyValue(T entity)
  {
    var idProperty = typeof(T).GetProperty("Id") ??
                     throw new InvalidOperationException("The entity must have an Id property");

    return idProperty.GetValue(entity);
  }
}