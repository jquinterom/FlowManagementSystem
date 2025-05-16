namespace FlowManagement.Core.Entities;

public class Flow
{
  public Guid Id { get; set; }
  public required string Name { get; set; }
  public required string Description { get; set; }
  public required string Purpose { get; set; }
  public ICollection<string> Steps { get; set; } = [];
  public bool IsActive { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
