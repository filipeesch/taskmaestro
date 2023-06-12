namespace TaskMaestro.Setup;

public class MaestroConfigurationBuilder
{
    private readonly List<MaestroQueue> queues = new() { new MaestroQueue(Constants.DefaultQueueName, 10) };

    public IReadOnlyList<MaestroQueue> Queues => this.queues;

    public Func<IServiceProvider, IMaestroDataStore> DataStoreFactory { get; private set; }

    public MaestroConfigurationBuilder AddQueue(string queueName, int workers)
    {
        this.queues.Add(new MaestroQueue(queueName, workers));
        return this;
    }

    public MaestroConfigurationBuilder UseDataStore(Func<IServiceProvider, IMaestroDataStore> factory)
    {
        this.DataStoreFactory = factory;
        return this;
    }
}
