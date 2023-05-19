namespace TaskMaestro;

public interface ITask
{
    Guid Id { get; }

    AckCode AckCode { get; }

    IReadOnlyList<AckCode> WaitForAcks { get; }

    Type HandlerType { get; }
}
