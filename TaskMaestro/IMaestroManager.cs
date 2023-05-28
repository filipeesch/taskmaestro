namespace TaskMaestro;

public interface IMaestroManager
{
    ITask GetTaskAsync(Guid id);

    Task SaveAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken = default);

    Task RegisterAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken = default);

    // methods to query tasks

    // methods to cancel tasks
}
