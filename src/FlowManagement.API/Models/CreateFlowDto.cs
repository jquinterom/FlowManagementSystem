namespace FlowManagement.API.Models;

public class CreateFlowDto
{
  public required string Name { get; set; }

  public required string Purpose { get; set; }

  public string? Description { get; set; }
}