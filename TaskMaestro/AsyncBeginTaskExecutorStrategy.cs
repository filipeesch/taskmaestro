namespace TaskMaestro;

internal class AsyncBeginTaskExecutorStrategy : ITaskExecutorStrategy
{
    public Task<object> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
