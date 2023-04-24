public interface IAsyncTaskBuilder<TIn, TOut, TBeginHandler, TEndHandler>
{
    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> EndsWith(IAckCode code);

    // IAsyncTaskBuilder<TInput, TAck, TOutput> WithErrorHandler<THandler>() where THandler : IAsyncTaskErrorHandler<TAck>;

    ITask<TIn, TOut> Create();
}

public interface IAsyncTaskBuilder<TIn, TOut, TBeginHandler>
{
    IAsyncTaskBuilder<TIn, TOut, TBeginHandler, TEndHandler> WithEndHandler<TEndHandler>()
        where TEndHandler : IAsyncTaskEndHandler<TOut>;

    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> EndsWith(IAckCode code);

    // IAsyncTaskBuilder<TInput, TAck, TOutput> WithErrorHandler<THandler>() where THandler : IAsyncTaskErrorHandler<TAck>;

    ITask<TIn, TOut> Create();
}
