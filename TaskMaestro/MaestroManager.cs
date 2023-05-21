namespace TaskMaestro;

public class MaestroManager : IMaestroManager
{
    private readonly IMaestroDataStore dataStore;

    public MaestroManager(IMaestroDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

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

    public async Task WaitForAsync(AckCode ackCode)
    {
        throw new NotImplementedException();
    }
}
