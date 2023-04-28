namespace TaskMaestro;

public interface IMaestroDataStore
{
    Task SaveTasksAsync(IEnumerable<ITask> tasks, CancellationToken? cancellationToken);

    Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken? cancellationToken);

    Task<ITask> ConsumeTaskAsync(CancellationToken token);
}
