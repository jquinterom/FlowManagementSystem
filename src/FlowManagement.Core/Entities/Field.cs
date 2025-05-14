namespace FlowManagement.Core.Entities;

public class Field
{
  public Guid Id { get; set; }
  public string Code { get; set; } // Ejemplo: F-0001
  public string Name { get; set; }
  public string Description { get; set; }
  public string DataType { get; set; } // string, int, bool, etc.
  public ICollection<StepInput> StepInputs { get; set; } = new List<StepInput>();
  public ICollection<StepOutput> StepOutputs { get; set; } = new List<StepOutput>();
}
