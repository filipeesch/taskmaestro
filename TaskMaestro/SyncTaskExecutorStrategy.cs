namespace TaskMaestro;

using System.Collections.Concurrent;

internal class SyncTaskExecutorStrategy : ITaskExecutorStrategy
{
    private readonly IMaestroManager manager;
    private static readonly ConcurrentDictionary<Type, IHandlerExecutor> Executors = new();

    public SyncTaskExecutorStrategy(IMaestroManager manager)
    {
        this.manager = manager;
    }

    public async Task<ITaskResult> ExecuteAsync(ITask task, object handler, IHandlerContext context, CancellationToken cancellationToken)
    {
        var syncTask = (SyncTask)task;

        var executor = Executors.GetOrAdd(
            handler.GetType(),
            _ => (IHandlerExecutor)Activator.CreateInstance(
                typeof(HandlerExecutor<,>).MakeGenericType(syncTask.InputType, syncTask.AckValueType))!);

        var result = await executor.ExecuteAsync(handler, syncTask.Input, context, cancellationToken);

        await this.manager.RegisterAcksAsync(new[] { new Ack(task.AckCode, result.AckValue) }, cancellationToken);

        return result;
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
            var h = (ISyncTaskHandler<TInput, TOutput>)handler;

            return await h.HandleAsync((TInput)input, context, cancellationToken);
        }
    }
}
