namespace TaskMaestro;

using System.Collections.Concurrent;

internal class AsyncBeginTaskExecutorStrategy : ITaskExecutorStrategy
{
    private readonly IMaestroManager manager;

    private static readonly ConcurrentDictionary<Type, IHandlerExecutor> Executors = new();

    public AsyncBeginTaskExecutorStrategy(IMaestroManager manager)
    {
        this.manager = manager;
    }

    public async Task<ITaskResult> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken)
    {
        var asyncBeginTask = (AsyncBeginTask)task;

        var executor = Executors.GetOrAdd(
            handler.GetType(),
            _ => (IHandlerExecutor)Activator.CreateInstance(
                typeof(HandlerExecutor<,>).MakeGenericType(asyncBeginTask.InputType, asyncBeginTask.AckValueType))!);

        var result = (AsyncBeginTaskResult)await executor.ExecuteAsync(handler, asyncBeginTask.Input, context, cancellationToken);

        await this.CreateEndTaskAsync(asyncBeginTask, result);

        return result;
    }

    private async Task CreateEndTaskAsync(AsyncBeginTask asyncBeginTask, AsyncBeginTaskResult asyncBeginResult)
    {
        var asyncEndTask = new AsyncEndTask(asyncBeginTask, asyncBeginResult.CompleteAckCodes);

        await this.manager.SaveAsync(asyncEndTask, CancellationToken.None);
    }

    private interface IHandlerExecutor
    {
        Task<ITaskResult> ExecuteAsync(
            object handler,
            object input,
            IHandlerContext context,
            CancellationToken cancellationToken);
    }

    private class HandlerExecutor<TInput, TOutput> : IHandlerExecutor
    {
        public async Task<ITaskResult> ExecuteAsync(
            object handler,
            object input,
            IHandlerContext context,
            CancellationToken cancellationToken)
        {
            var h = (IAsyncTaskHandler<TInput, TOutput>)handler;

            return await h.HandleBeginAsync((TInput)input, context, cancellationToken);
        }
    }
}
