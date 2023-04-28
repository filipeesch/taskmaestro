public interface IAsyncTaskErrorHandler<in TAcknowledge>
{
    Task HandleErrorAsync(TAcknowledge acknowledge, CancellationToken cancellationToken);
}
