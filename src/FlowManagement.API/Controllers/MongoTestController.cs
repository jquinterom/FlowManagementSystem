using FlowManagement.Core.Entities;
using FlowManagement.Infrastructure.Data;
using FlowManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/mongo-test")]
public class MongoTestController : ControllerBase
{
  private readonly MongoHealthService _healthService;

  public MongoTestController(MongoHealthService healthService)
  {
    _healthService = healthService;
  }

  [HttpGet("health")]
  public async Task<IResult> CheckMongoHealth()
  {
    var isHealthy = await _healthService.CheckDatabaseConnectionAsync();
    return isHealthy
        ? TypedResults.Ok(new { Status = "Healthy" })
        : TypedResults.Problem("Service unavailable", statusCode: 503);
  }

  [HttpPost("test-data")]
  public async Task<IActionResult> CreateTestData([FromServices] MongoDbContext context)
  {
    var testFlow = new Flow
    {
      Id = Guid.NewGuid(),
      Name = "Flujo de Prueba",
      Description = "Este es un flujo de prueba",
      Purpose = "Verificar conexión con MongoDB Atlas",
      IsActive = true,
      CreatedAt = DateTime.UtcNow
    };

    await context.Flows.InsertOneAsync(testFlow);

    return Ok(new
    {
      Success = true,
      FlowId = testFlow.Id,
      Message = "Datos de prueba creados exitosamente"
    });
  }
}