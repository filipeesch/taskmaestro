namespace TaskMaestro;

using System.Collections.Concurrent;

internal class SyncTaskExecutorStrategy : ITaskExecutorStrategy
{
    private static readonly ConcurrentDictionary<Type, IHandlerExecutor> Executors = new();

    public async Task<object> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken)
    {
        var syncTask = (SyncTask)task;

        var executor = Executors.GetOrAdd(
            handler.GetType(),
            _ => (IHandlerExecutor)Activator.CreateInstance(
                typeof(HandlerExecutor<,>).MakeGenericType(syncTask.InputType, syncTask.AckValueType))!);

        return await executor.ExecuteAsync(handler, syncTask.Input, context, cancellationToken);
    }

    private interface IHandlerExecutor
    {
        Task<object> ExecuteAsync(
            object handler,
            object input,
            IHandlerContext context,
            CancellationToken cancellationToken);
    }

    private class HandlerExecutor<TInput, TOutput> : IHandlerExecutor
    {
        public async Task<object> ExecuteAsync(
            object handler,
            object input,
            IHandlerContext context,
            CancellationToken cancellationToken)
        {
            var h = (ISyncTaskHandler<TInput, TOutput>)handler;

            return await h.HandleAsync((TInput)input, context, cancellationToken);
        }
    }
}
