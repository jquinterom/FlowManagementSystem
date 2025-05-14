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
  public ICollection<StepInput> Inputs { get; set; } = new List<StepInput>();
  public ICollection<StepOutput> Outputs { get; set; } = new List<StepOutput>();
  public bool IsParallel { get; set; } // Indica si puede ejecutarse en paralelo
  public string ActionType { get; set; } // Tipo de acción que realiza el paso
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
