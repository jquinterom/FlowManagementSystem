using System.Linq.Expressions;
using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Interfaces;

public interface IFieldRepository : IRepository<Field>
{
  Task<Field> GetByCodeAsync(string code);
  Task<bool> AnyAsync(Expression<Func<Field, bool>> predicate);
}