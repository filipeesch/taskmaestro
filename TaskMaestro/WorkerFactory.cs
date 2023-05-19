namespace TaskMaestro;

public class WorkerFactory : IWorkerFactory
{
    private readonly IMaestroDataStore dataStore;
    private readonly ITaskExecutor executor;

    public WorkerFactory(IMaestroDataStore dataStore, ITaskExecutor executor)
    {
        this.dataStore = dataStore;
        this.executor = executor;
    }

    public IMaestroWorker Create(string queueName) =>
        new MaestroWorker(this.dataStore, this.executor, queueName);
}
