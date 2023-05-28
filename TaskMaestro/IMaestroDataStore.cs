namespace TaskMaestro;

public interface IMaestroDataStore
{
    Task SaveTasksAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken);

    Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken);

    Task CompleteTaskAsync(TaskExecutionReport report, CancellationToken cancellationToken);

    IAsyncEnumerable<ITask> ConsumeTasksAsync(string queueName, CancellationToken token);

    IAsyncEnumerable<object> GetAckValueByTypesAsync(Guid taskId, IEnumerable<Type> types, CancellationToken cancellationToken);
}
