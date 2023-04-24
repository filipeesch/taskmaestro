public interface ISyncTaskHandler<in TInput, TOutput>
{
    Task<TaskResult<TOutput>> HandleAsync(TInput input, CancellationToken cancellationToken);
}