public interface IAsyncTaskBeginHandler<in TInput>
{
    Task HandleBeginAsync(TInput input, CancellationToken cancellationToken);
}