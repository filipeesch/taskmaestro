namespace TaskMaestro;

public class AsyncEndTask : ITask
{
    public AsyncEndTask(
        Type ackValueType,
        IReadOnlyList<AckCode> waitForAcks,
        Type? handlerType)
    {
        this.Id = Guid.NewGuid();
        this.AckCode = AckCode.FromGuid(Guid.NewGuid());
        this.AckValueType = ackValueType;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CreatedAt = DateTime.UtcNow;
    }

    public AsyncEndTask(
        Guid id,
        AckCode ackCode,
        Type ackValueType,
        IReadOnlyList<AckCode> waitForAcks,
        Type? handlerType,
        DateTime createdAt,
        DateTime? fetchedAt,
        DateTime? completedAt)
    {
        this.Id = id;
        this.AckCode = ackCode;
        this.AckValueType = ackValueType;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CreatedAt = createdAt;
        this.FetchedAt = fetchedAt;
        this.CompletedAt = completedAt;
    }

    public Guid Id { get; }

    public AckCode AckCode { get; }

    public Type AckValueType { get; }

    public IReadOnlyList<AckCode> WaitForAcks { get; }

    public Type? HandlerType { get; }

    public object Input { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }
}
