namespace TaskMaestro;

public class TaskExecutor : ITaskExecutor
{
    private readonly IMaestroManager manager;
    private readonly IMaestroDataStore dataStore;
    private readonly IServiceProvider serviceProvider;
    private readonly Dictionary<Type, ITaskExecutorStrategy> strategies;

    public TaskExecutor(IMaestroManager manager, IMaestroDataStore dataStore, IServiceProvider serviceProvider)
    {
        this.manager = manager;
        this.dataStore = dataStore;
        this.serviceProvider = serviceProvider;
        this.strategies = new Dictionary<Type, ITaskExecutorStrategy>
        {
            { typeof(SyncTask), new SyncTaskExecutorStrategy(this.manager) },
            { typeof(AsyncBeginTask), new AsyncBeginTaskExecutorStrategy(this.manager) },
            { typeof(AsyncEndTask), new AsyncEndTaskExecutorStrategy(this.manager) },
        };
    }

    public async Task ExecuteAsync(ITask task, IHandlerContext context, CancellationToken cancellationToken)
    {
        var handler = this.serviceProvider.GetService(task.HandlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"Handler '{task.HandlerType.FullName}' is not registered in the DI container");
        }

        TaskExecutionReport report;

        try
        {
            var result = await this.strategies[task.GetType()].ExecuteAsync(task, handler, context, cancellationToken);
            report = new TaskExecutionReport(task.Id, TaskExecutionReportType.Success, string.Empty);
        }
        catch (Exception e)
        {
            report = new TaskExecutionReport(task.Id, TaskExecutionReportType.Error, e.ToString());
        }

        await this.dataStore.CompleteTaskAsync(report, cancellationToken);
    }
}
