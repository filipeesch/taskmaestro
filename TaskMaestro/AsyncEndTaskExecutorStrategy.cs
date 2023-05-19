namespace TaskMaestro;

internal class AsyncEndTaskExecutorStrategy : ITaskExecutorStrategy
{
    public Task<object> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
