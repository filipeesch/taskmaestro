public interface IAsyncTaskHandler<in TInput, TAckValue>
{
    Task<AsyncBeginResult> HandleBeginAsync(TInput input, IHandlerContext context, CancellationToken cancellationToken);

    Task<TAckValue> HandleEndAsync(IHandlerContext context, CancellationToken cancellationToken);
}
