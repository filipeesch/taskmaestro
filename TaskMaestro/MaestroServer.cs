namespace TaskMaestro;

public class MaestroServer : IMaestroServer
{
    private readonly IWorkerFactory workerFactory;
    private readonly MaestroQueue[] queues;

    private readonly List<IMaestroWorker> workers = new();
    private readonly SemaphoreSlim semaphore = new(1, 1);

    public MaestroServer(
        IWorkerFactory workerFactory,
        MaestroQueue[] queues)
    {
        this.workerFactory = workerFactory;
        this.queues = queues;
    }

    public async Task StartAsync()
    {
        await this.semaphore.WaitAsync();

        try
        {
            foreach (var queue in this.queues)
            {
                for (int i = 0; i < queue.Workers; i++)
                {
                    this.workers.Add(this.workerFactory.Create(queue.Name));
                }
            }

            foreach (var worker in this.workers)
            {
                await worker.StartAsync();
            }
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public async Task StopAsync()
    {
        await this.semaphore.WaitAsync();

        try
        {
            await Task.WhenAll(this.workers.Select(worker => worker.StopAsync()));
            this.workers.ForEach(worker => worker.Dispose());

            this.workers.Clear();
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public void Dispose()
    {
        this.semaphore.Dispose();
    }
}
