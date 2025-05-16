using System.ComponentModel.DataAnnotations;
using FlowManagement.Core.Entities;

namespace FlowManagement.Application.DTOs.Steps
{
  public class CreateStepDto
  {
    [Required]
    [RegularExpression(@"^SP-\d{4}$", ErrorMessage = "The step code must be in the format SP-XXXX")]
    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public int Order { get; set; }

    public bool IsParallel { get; set; }

    public ActionType ActionType { get; set; }

    public required List<StepInputDto> Inputs { get; set; } = [];
    public required List<StepOutputDto> Outputs { get; set; } = [];
  }


  public class StepInputDto
  {
    [Required]
    [RegularExpression(@"^F-\d{4}$", ErrorMessage = "The field code must be in the format F-XXXX")]
    public required string FieldCode { get; set; }
    public bool IsRequired { get; set; }
  }

  public class StepOutputDto
  {
    [Required]
    [RegularExpression(@"^F-\d{4}$", ErrorMessage = "The field code must be in the format F-XXXX")]
    public required string FieldCode { get; set; }
  }
}