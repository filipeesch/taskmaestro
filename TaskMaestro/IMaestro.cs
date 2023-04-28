using TaskMaestro;
using TaskMaestro.Builders;

public interface IMaestro
{
    ITaskGroupBuilder BuildTaskGroup();

    ITaskBuilder BuildTask();

    ITask GetTaskAsync(Guid id);

    Task SaveAsync(IEnumerable<ITask> tasks, CancellationToken? cancellationToken = null);

    Task RegisterAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken);

    // methods to query tasks

    // methods to cancel tasks
}
