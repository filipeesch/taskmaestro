namespace TaskMaestro;

internal interface ITaskExecutorStrategy
{
    Task<object> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken);
}