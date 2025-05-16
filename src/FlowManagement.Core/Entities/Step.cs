using System.ComponentModel.DataAnnotations;

namespace FlowManagement.Core.Entities;

public class Step
{
  public Guid Id { get; set; }
  public string Code { get; set; } // Ejemplo: STP-0001

  [Required]
  public string Name { get; set; }
  public string Description { get; set; }

  [Range(1, int.MaxValue)]
  public int Order { get; set; }
  public ICollection<StepInput> Inputs { get; set; } = [];
  public ICollection<StepOutput> Outputs { get; set; } = [];
  public bool IsParallel { get; set; } // Indica si puede ejecutarse en paralelo
  public ActionType ActionType { get; set; } // Tipo de acción que realiza el paso
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public List<StepDependency> Dependencies { get; set; } = new();
}

public class StepDependency
{
  public Guid StepId { get; set; }  // Paso del que depende
  public string RequiredOutput { get; set; }  // Campo output requerido
}


public enum ActionType
{
  DocumentValidation,
  EmailNotification,
}