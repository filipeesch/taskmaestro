namespace TaskMaestro;

public class TaskExecutionReport
{
    public TaskExecutionReport(
        Guid taskId,
        TaskExecutionReportType type,
        string message)
    {
        this.TaskId = taskId;
        this.Type = type;
        this.Message = message;
        this.CreatedAt = DateTime.UtcNow;
    }

    public Guid TaskId { get; }

    public TaskExecutionReportType Type { get; }

    public string Message { get; }

    public DateTime CreatedAt { get; }
}
