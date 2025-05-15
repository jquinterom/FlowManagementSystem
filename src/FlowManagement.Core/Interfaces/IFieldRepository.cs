using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IFieldRepository : IRepository<Field>
{
  Task<Field> GetByCodeAsync(string code);
  Task<IEnumerable<Field>> GetFieldsByDataTypeAsync(string dataType);
}