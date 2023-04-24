public interface IAsyncTaskEndHandler<TAckValue>
{
    Task<TAckValue> HandleEndAsync(CancellationToken cancellationToken);
}