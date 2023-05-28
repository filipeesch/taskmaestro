namespace TaskMaestro;

public class SyncTask : ITask
{
    public SyncTask(
        object input,
        Type ackValueType,
        Guid? groupId,
        IReadOnlyList<AckCode> waitForAcks,
        Type? handlerType,
        string queue,
        int maxRetryCount)
    {
        this.Id = GuidGenerator.New();
        this.AckCode = AckCode.FromGuid(GuidGenerator.New());
        this.AckValueType = ackValueType;
        this.Input = input;
        this.InputType = input.GetType();
        this.GroupId = groupId;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.Queue = queue;
        this.CreatedAt = DateTime.UtcNow;
        this.Status = TaskStatus.Created;
        this.MaxRetryCount = maxRetryCount;
    }

    public SyncTask(
        Guid id,
        AckCode ackCode,
        object input,
        Type ackValueType,
        Guid? groupId,
        IReadOnlyList<AckCode> waitForAcks,
        Type handlerType,
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
        this.Input = input;
        this.InputType = input.GetType();
        this.AckValueType = ackValueType;
        this.GroupId = groupId;
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

    public object Input { get; }

    public Type InputType { get; }

    public Type AckValueType { get; }

    public Guid? GroupId { get; }

    public IReadOnlyCollection<AckCode> WaitForAcks { get; }

    public Type HandlerType { get; }

    public string Queue { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }

    public TaskStatus Status { get; }

    public int MaxRetryCount { get; }

    public int CurrentRetryCount { get; }
}
