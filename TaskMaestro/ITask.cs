namespace TaskMaestro;

public interface ITask
{
    Guid Id { get; }

    AckCode AckCode { get; }

    IReadOnlyCollection<AckCode> WaitForAcks { get; }

    Type HandlerType { get; }
}
