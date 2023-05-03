namespace TaskMaestro.DataStore.SqlServer;

public class SqlServerDataStore : IMaestroDataStore
{
    private readonly string connectionString;

    public SqlServerDataStore(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Task SaveTasksAsync(IEnumerable<ITask> tasks, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ITask> ConsumeTaskAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
