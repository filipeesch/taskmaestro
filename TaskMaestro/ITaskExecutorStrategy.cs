namespace TaskMaestro;

internal interface ITaskExecutorStrategy
{
    Task<ITaskResult> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken);
}
