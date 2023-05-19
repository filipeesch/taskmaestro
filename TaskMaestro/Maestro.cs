namespace TaskMaestro;

using TaskMaestro.Builders;

public class Maestro : IMaestro
{
    private readonly IMaestroDataStore dataStore;

    public Maestro(IMaestroDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public ITaskGroupBuilder BuildTaskGroup()
    {
        throw new NotImplementedException();
    }

    public ITaskBuilder BuildTask() => new TaskBuilder<Void, Void>();

    public ITask GetTaskAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(
        IReadOnlyCollection<ITask> tasks,
        CancellationToken cancellationToken = default)
    {
        await this.dataStore.SaveTasksAsync(tasks, cancellationToken);
    }

    public async Task RegisterAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken)
    {
        await this.dataStore.SaveAcksAsync(acks, cancellationToken);
    }
}
