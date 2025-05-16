namespace FlowManagement.Application.DTOs.Steps
{
  public class UpdateStepDto
  {
    public required List<StepInputDto> Inputs { get; set; } = [];
    public required List<StepOutputDto> Outputs { get; set; } = [];
  }
}