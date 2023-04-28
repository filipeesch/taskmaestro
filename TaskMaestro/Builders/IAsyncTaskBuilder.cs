using TaskMaestro;

public interface IAsyncTaskBuilder<TIn, TOut>
{
    IAsyncTaskBuilder<TIn, TOut> EndsWith(AckCode code);

    IAsyncTaskBuilder<TIn, TOut> EndsWith(params AckCode[] codes);

    // IAsyncTaskBuilder<TInput, TAck, TOutput> WithErrorHandler<THandler>() where THandler : IAsyncTaskErrorHandler<TAck>;

    ITask Create();
}
