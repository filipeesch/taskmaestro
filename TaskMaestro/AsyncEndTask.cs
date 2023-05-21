namespace TaskMaestro;

public class AsyncEndTask : ITask
{
    public AsyncEndTask(
        AckCode ackCode,
        Type ackValueType,
        IReadOnlyCollection<AckCode> waitForAcks,
        Type? handlerType,
        Type inputType,
        object input,
        DateTime createdAt)
    {
        this.Id = Guid.NewGuid();
        this.AckCode = ackCode;
        this.AckValueType = ackValueType;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.InputType = inputType;
        this.Input = input;
        this.CreatedAt = createdAt;
    }

    public AsyncEndTask(
        Guid id,
        AckCode ackCode,
        Type ackValueType,
        object input,
        Type inputType,
        IReadOnlyCollection<AckCode> waitForAcks,
        Type? handlerType,
        DateTime createdAt,
        DateTime? fetchedAt,
        DateTime? completedAt)
    {
        this.Id = id;
        this.AckCode = ackCode;
        this.AckValueType = ackValueType;
        this.Input = input;
        this.InputType = inputType;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CreatedAt = createdAt;
        this.FetchedAt = fetchedAt;
        this.CompletedAt = completedAt;
    }

    public Guid Id { get; }

    public AckCode AckCode { get; }

    public Type AckValueType { get; }

    public object Input { get; }

    public Type InputType { get; }

    public IReadOnlyCollection<AckCode> WaitForAcks { get; }

    public Type? HandlerType { get; }

    public DateTime CreatedAt { get; }

    public DateTime? FetchedAt { get; }

    public DateTime? CompletedAt { get; }
}
