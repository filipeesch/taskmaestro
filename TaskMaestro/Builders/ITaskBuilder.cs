namespace TaskMaestro.Builders;

using TaskMaestro;

public interface ITaskBuilder
{
    ITaskBuilder InGroup(ITaskGroup? taskGroup);

    ITaskBuilder<TIn> Input<TIn>(TIn input);

    // options here, like retries

    // metadata here, like headers
}

public interface ITaskBuilder<TIn>
{
    ITaskBuilder<TIn> WaitFor(AckCode code);

    ITaskBuilder<TIn> WaitFor(params AckCode[] codes);

    ITaskBuilder<TIn, TAckValue> Produces<TAckValue>();

    ISyncTaskBuilder<TIn, Void> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, Void>;

    IAsyncTaskBuilder<TIn, Void> Async<THandler>()
        where THandler : IAsyncTaskHandler<TIn, Void>;
}

public interface ITaskBuilder<TIn, TAckValue>
{
    ISyncTaskBuilder<TIn, TAckValue> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, TAckValue>;

    IAsyncTaskBuilder<TIn, TAckValue> Async<THandler>()
        where THandler : IAsyncTaskHandler<TIn, TAckValue>;
}
