using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services;

public class FieldService(IFieldRepository fieldRepository, ILogger<FieldService> logger) : IFieldService
{
  private readonly IFieldRepository _fieldRepository = fieldRepository;
  private readonly ILogger<FieldService> _logger = logger;

  public async Task<Field> CreateFieldAsync(string code, string name, string dataType, string? description = null)
  {
    if (await _fieldRepository.GetByCodeAsync(code) != null)
    {
      throw new InvalidOperationException($"The field with code {code} already exists");
    }

    var field = new Field
    {
      Id = Guid.NewGuid(),
      Code = code,
      Name = name,
      DataType = dataType,
      Description = description ?? string.Empty,
    };

    await _fieldRepository.AddAsync(field);
    _logger.LogInformation("Field created with id {field.Id}", field.Id);

    return field;
  }

  public async Task<IEnumerable<Field>> GetAllFieldsAsync()
  {
    return await _fieldRepository.GetAllAsync();
  }

  public async Task<Field> GetFieldAsync(Guid id)
  {
    return await _fieldRepository.GetByIdAsync(id);
  }

  public async Task UpdateFieldAsync(Guid id, string name, string description)
  {
    var field = await _fieldRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Field not found");
    field.Name = name;
    field.Description = description;

    await _fieldRepository.UpdateAsync(field);
  }
}