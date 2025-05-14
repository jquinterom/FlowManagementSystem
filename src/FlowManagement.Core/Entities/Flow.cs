namespace FlowManagement.Core.Entities;
public class Flow
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public string Purpose { get; set; }
  public ICollection<Step> Steps { get; set; } = new List<Step>();
  public bool IsActive { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
