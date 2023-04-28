public interface IAsyncTaskHandler<in TInput, TAckValue>
{
    Task HandleBeginAsync(TInput input, IHandlerContext context, CancellationToken cancellationToken);

    Task<TAckValue> HandleEndAsync(IHandlerContext context, CancellationToken cancellationToken);
}
