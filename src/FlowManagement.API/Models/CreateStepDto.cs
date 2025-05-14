namespace FlowManagement.API.Models;

public class CreateStepDto
{
  public required string Code { get; set; }

  public required string Name { get; set; }

  public string? Description { get; set; }

  public int Order { get; set; }

  public bool IsParallel { get; set; }

  public string ActionType { get; set; }
}