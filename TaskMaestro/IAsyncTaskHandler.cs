namespace TaskMaestro;

public interface IAsyncTaskHandler<in TInput, TAckValue>
{
    Task<AsyncBeginTaskResult> HandleBeginAsync(TInput input, IHandlerContext context, CancellationToken cancellationToken);

    Task<AsyncEndTaskResult<TAckValue>> HandleEndAsync(TInput input, IHandlerContext context, CancellationToken cancellationToken);
}
