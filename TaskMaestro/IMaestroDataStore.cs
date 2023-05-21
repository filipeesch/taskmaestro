namespace TaskMaestro;

public interface IMaestroDataStore
{
    Task SaveTasksAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken);

    Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken);

    IAsyncEnumerable<ITask> ConsumeTasksAsync(string queueName, CancellationToken token);
}
