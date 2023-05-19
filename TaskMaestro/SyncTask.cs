namespace TaskMaestro;

public class SyncTask : ITask
{
    public SyncTask(
        object input,
        Type ackValueType,
        Guid? groupId,
        IReadOnlyList<AckCode> waitForAcks,
        Type? handlerType)
    {
        this.Id = Guid.NewGuid();
        this.AckCode = AckCode.FromGuid(Guid.NewGuid());
        this.Input = input;
        this.InputType = input.GetType();
        this.AckValueType = ackValueType;
        this.GroupId = groupId;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CreatedAt = DateTime.UtcNow;
    }

    public SyncTask(
        Guid id,
        AckCode ackCode,
        object input,
        Type ackValueType,
        Guid? groupId,
        IReadOnlyList<AckCode> waitForAcks,
        Type handlerType,
        DateTime createdAt,
        DateTime? fetchedAt,
        DateTime? completedAt)
    {
        this.Id = id;
        this.AckCode = ackCode;
        this.Input = input;
        this.InputType = input.GetType();
        this.AckValueType = ackValueType;
        this.GroupId = groupId;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CreatedAt = createdAt;
        this.FetchedAt = fetchedAt;
        this.CompletedAt = completedAt;
    }

    public Guid Id { get; }

    public AckCode AckCode { get; }

    public object Input { get; }

    public Type InputType { get; }

    public Type AckValueType { get; }

    public Guid? GroupId { get; }

    public IReadOnlyList<AckCode> WaitForAcks { get; }

    public Type HandlerType { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }
}
