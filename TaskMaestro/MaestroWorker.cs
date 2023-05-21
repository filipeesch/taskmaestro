namespace TaskMaestro;

public class MaestroWorker : IMaestroWorker
{
    private readonly IMaestroDataStore dataStore;
    private readonly ITaskExecutor executor;
    private readonly string queueName;

    private Task? backgroundTask;
    private CancellationTokenSource? stopCancellationTokenSource;

    private readonly SemaphoreSlim semaphore = new(1, 1);

    public MaestroWorker(IMaestroDataStore dataStore, ITaskExecutor executor, string queueName)
    {
        this.dataStore = dataStore;
        this.executor = executor;
        this.queueName = queueName;
    }

    public async Task StartAsync()
    {
        await this.semaphore.WaitAsync();

        try
        {
            this.stopCancellationTokenSource = new CancellationTokenSource();

            this.backgroundTask = Task.Run(
                async () =>
                {
                    try
                    {
                        await foreach (var task in this.dataStore.ConsumeTasksAsync(
                                           this.queueName,
                                           this.stopCancellationTokenSource.Token))
                        {
                            var context = new DefaultHandlerContext(this.dataStore, task);
                            await this.executor.ExecuteAsync(task, context, this.stopCancellationTokenSource.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Do nothing
                    }
                });
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
            if (this.backgroundTask is null)
            {
                return;
            }

            this.stopCancellationTokenSource?.Cancel();

            await this.backgroundTask;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public void Dispose()
    {
        this.backgroundTask?.Dispose();
        this.stopCancellationTokenSource?.Dispose();
        this.semaphore.Dispose();
    }
}
