namespace FlowManagement.Core.Entities;

public class Field
{
  public Guid Id { get; set; }
  public required string Code { get; set; } // Ejemplo: F-0001
  public required string Name { get; set; }
  public required string Description { get; set; }
  public required string DataType { get; set; } // string, int, bool, etc.
  public ICollection<StepInput> StepInputs { get; set; } = [];
  public ICollection<StepOutput> StepOutputs { get; set; } = [];
}
