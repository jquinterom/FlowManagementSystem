namespace FlowManagement.Core.Entities;
public class StepInput
{
  public Guid StepId { get; set; }
  public Step Step { get; set; }
  public Guid FieldId { get; set; }
  public Field Field { get; set; }
  public bool IsRequired { get; set; }
}
