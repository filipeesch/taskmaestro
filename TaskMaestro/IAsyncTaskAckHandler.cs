public interface IAsyncTaskAckHandler<in TAck, TOutput>
{
    Task<TaskResult<TOutput>> HandleEndAsync(TAck acknowledge, CancellationToken cancellationToken);
}