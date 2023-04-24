public interface ITaskBuilder
{
    ITaskBuilder InGroup(ITaskGroup? taskGroup);

    ITaskBuilder<TIn> Input<TIn>(TIn input);

    // options here, like retries

    // metadata here, like headers
}

public interface ITaskBuilder<TIn>
{
    ITaskBuilder<TIn> WaitFor(params IAckCode[] code);

    ITaskBuilder<TIn, TOut> Produces<TOut>();

    ISyncTaskBuilder<TIn, Void, THandler> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, Void>;

    IAsyncTaskBuilder<TIn, Void, TBeginHandler> Async<TBeginHandler>()
        where TBeginHandler : IAsyncTaskBeginHandler<TIn>;
}

public interface ITaskBuilder<TIn, TOut>
{
    ISyncTaskBuilder<TIn, TOut, THandler> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, TOut>;

    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> Async<TBeginHandler>()
        where TBeginHandler : IAsyncTaskBeginHandler<TIn>;
}
