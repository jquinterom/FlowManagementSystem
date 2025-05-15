using Microsoft.Extensions.Options;
using FlowManagement.Infrastructure.Services;
using FlowManagement.Infrastructure.Data;
using DotNetEnv;
using MongoDB.Driver;
using FlowManagement.Core.Interfaces;
using FlowManagement.Infrastructure.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using FlowManagement.Infrastructure.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

Env.Load();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ?? "";
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME") ?? "";


if (!string.IsNullOrEmpty(mongoConnectionString) &&
    !string.IsNullOrEmpty(mongoDatabaseName))
{
  builder.Services.Configure<MongoDbSettings>(options =>
    {
      options.ConnectionString = mongoConnectionString;
      options.DatabaseName = mongoDatabaseName;
    });

  builder.Services.AddSingleton<IMongoClient>(sp =>
{
  var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
  return new MongoClient(settings.ConnectionString);
});
}

builder.Services.AddSingleton<MongoDbContext>();

// Repositories
builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IFlowExecutionRepository, FlowExecutionRepository>();

// Services
builder.Services.AddScoped<IFlowService, FlowService>();
builder.Services.AddScoped<IStepService, StepService>();
builder.Services.AddScoped<IFieldService, FieldService>();
builder.Services.AddScoped<IFlowExecutionService, FlowExecutionService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new()
  {
    Title = builder.Environment.ApplicationName,
    Version = "v1"
  });
});

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.ConfigObject.DisplayRequestDuration = true;
  });
}

app.UseHttpsRedirection();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.Run();
