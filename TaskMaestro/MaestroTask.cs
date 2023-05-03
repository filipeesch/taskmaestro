namespace TaskMaestro;

public class MaestroTask : ITask
{
    public MaestroTask(
        TaskType type,
        object input,
        Type ackValueType,
        ITaskGroup? group,
        IReadOnlyList<AckCode> waitForAcks,
        Type? handlerType,
        IReadOnlyList<AckCode> completeAcks)
    {
        this.Id = Guid.NewGuid();
        this.AckCode = AckCode.FromGuid(Guid.NewGuid());
        this.Type = type;
        this.Input = input;
        this.InputType = input.GetType();
        this.AckValueType = ackValueType;
        this.Group = group;
        this.WaitForAcks = waitForAcks;
        this.HandlerType = handlerType;
        this.CompleteAcks = completeAcks;
    }

    public Guid Id { get; }

    public AckCode AckCode { get; }

    public TaskType Type { get; }

    public object Input { get; }

    public Type InputType { get; }

    private Type AckValueType { get; }

    private ITaskGroup? Group { get; }

    public IReadOnlyList<AckCode> WaitForAcks { get; }

    private Type? HandlerType { get; }

    private IReadOnlyList<AckCode> CompleteAcks { get; }
}
