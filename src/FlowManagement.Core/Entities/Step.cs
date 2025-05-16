using System.ComponentModel.DataAnnotations;

namespace FlowManagement.Core.Entities;

public class Step
{
  public Guid Id { get; set; }
  public required string Code { get; set; } // Ejemplo: STP-0001

  [Required]
  public required string Name { get; set; }
  public required string Description { get; set; }

  [Range(1, int.MaxValue)]
  public int Order { get; set; }
  public ICollection<StepInput> Inputs { get; set; } = [];
  public ICollection<StepOutput> Outputs { get; set; } = [];
  public bool IsParallel { get; set; }
  public ActionType ActionType { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public List<StepDependency> Dependencies { get; set; } = new();
}

public class StepDependency
{
  public required string StepCode { get; set; }
  public required string RequiredOutput { get; set; }
}


public enum ActionType
{
  DocumentValidation,
  EmailNotification,
}