namespace TaskMaestro;

public interface ITask
{
    Guid Id { get; }

    AckCode AckCode { get; }

    IReadOnlyCollection<AckCode> WaitForAcks { get; }

    Type HandlerType { get; }

    public string Queue { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }

    public TaskStatus Status { get; }

    public int MaxRetryCount { get; }

    public int CurrentRetryCount { get; }
}
