using FlowManagement.Core.Entities;

namespace FlowManagement.Core.Models
{
  public class CreateStepDto
  {
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
    public string FieldCode { get; set; }
    public bool IsRequired { get; set; }
  }

  public class StepOutputDto
  {
    public string FieldCode { get; set; }
  }
}