namespace FlowManagement.Core.Entities;


public class FlowExecution
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid ExecutionId { get; set; } = Guid.NewGuid();

  public Guid FlowId { get; set; }
  public DateTime StartTime { get; set; } = DateTime.UtcNow;
  public DateTime? EndTime { get; set; }

  public ExecutionStatus Status { get; set; } = ExecutionStatus.Running;

  public List<StepExecution> StepExecutions { get; set; } = new();
  public Dictionary<string, object> FinalOutputs { get; set; } = new();
}

public class StepExecution
{
  public Guid StepId { get; set; }
  public DateTime ExecutionTime { get; set; } = DateTime.UtcNow;

  public ExecutionStatus Status { get; set; }

  public string? Error { get; set; }
  public Dictionary<string, object>? Outputs { get; set; }
}

public enum ExecutionStatus
{
  Running,
  Completed,
  Failed,
  Timeout
}