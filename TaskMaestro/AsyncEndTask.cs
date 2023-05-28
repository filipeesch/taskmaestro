namespace TaskMaestro;

public class AsyncEndTask : ITask
{
    public AsyncEndTask(AsyncBeginTask beginTask, IReadOnlyCollection<AckCode> waitForAckCodes)
    {
        this.Id = GuidGenerator.New();
        this.AckCode = beginTask.AckCode;
        this.AckValueType = beginTask.AckValueType;
        this.WaitForAcks = waitForAckCodes;
        this.HandlerType = beginTask.HandlerType;
        this.InputType = beginTask.InputType;
        this.Input = beginTask.Input;
        this.Queue = beginTask.Queue;
        this.CreatedAt = beginTask.CreatedAt;
        this.Status = TaskStatus.Created;
        this.MaxRetryCount = beginTask.MaxRetryCount;
    }

    public AsyncEndTask(
        Guid id,
        AckCode ackCode,
        Type ackValueType,
        object input,
        Type inputType,
        IReadOnlyCollection<AckCode> waitForAcks,
        Type? handlerType,
        string queue,
        DateTime createdAt,
        DateTime? fetchedAt,
        DateTime? completedAt,
        TaskStatus status,
        int maxRetryCount,
        int currentRetryCount)
    {
        this.Id = id;
        this.AckCode = ackCode;
        this.AckValueType = ackValueType;
        this.Input = input;
        this.InputType = inputType;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.Queue = queue;
        this.CreatedAt = createdAt;
        this.FetchedAt = fetchedAt;
        this.CompletedAt = completedAt;
        this.Status = status;
        this.MaxRetryCount = maxRetryCount;
        this.CurrentRetryCount = currentRetryCount;
    }

    public Guid Id { get; }

    public AckCode AckCode { get; }

    public Type AckValueType { get; }

    public object Input { get; }

    public string Queue { get; }

    public Type InputType { get; }

    public IReadOnlyCollection<AckCode> WaitForAcks { get; }

    public Type? HandlerType { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }

    public TaskStatus Status { get; }

    public int MaxRetryCount { get; }

    public int CurrentRetryCount { get; }
}
