namespace FlowManagement.Core.Entities;
public class StepOutput
{
  public Guid StepId { get; set; }
  public Step Step { get; set; }
  public Guid FieldId { get; set; }
  public Field Field { get; set; }
}
