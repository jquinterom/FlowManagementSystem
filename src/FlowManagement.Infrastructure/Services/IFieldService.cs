using FlowManagement.Core.Entities;

namespace FlowManagement.Infrastructure.Services;

public interface IFieldService
{
  Task<Field> CreateFieldAsync(string code, string name, string dataType, string? description = null);
  Task<IEnumerable<Field>> GetAllFieldsAsync();
  Task<Field> GetFieldAsync(Guid id);
  Task UpdateFieldAsync(Guid id, string name, string description);
}