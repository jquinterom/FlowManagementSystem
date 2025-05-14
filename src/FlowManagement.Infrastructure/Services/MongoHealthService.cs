using FlowManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FlowManagement.Infrastructure.Services;

public class MongoHealthService
{
  private readonly MongoDbContext _context;
  private readonly ILogger<MongoHealthService> _logger;

  public MongoHealthService(MongoDbContext context, ILogger<MongoHealthService> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<bool> CheckDatabaseConnectionAsync()
  {
    try
    {
      // Intenta listar las colecciones como prueba de conexión
      using var cursor = await _context.Flows.Database.ListCollectionNamesAsync();
      await cursor.FirstOrDefaultAsync();
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al conectar con MongoDB Atlas");
      return false;
    }
  }
}