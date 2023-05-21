namespace TaskMaestro;

public interface ISyncTaskHandler<in TInput, TOutput>
{
    Task<SyncTaskResult<TOutput>> HandleAsync(TInput input, IHandlerContext context, CancellationToken cancellationToken);
}

public interface ITaskResult
{
    object AckValue { get; }
}

public class SyncTaskResult<TAckType> : ITaskResult
{
    public SyncTaskResult(TAckType ackValue)
    {
        this.AckValue = ackValue;
    }

    object ITaskResult.AckValue => this.AckValue;

    public TAckType AckValue { get; }
}

public record AsyncBeginTaskResult(params AckCode[] CompleteAckCodes) : ITaskResult
{
    public object AckValue => Void.Value;
}

public class AsyncEndTaskResult<TAckType> : ITaskResult
{
    public AsyncEndTaskResult(TAckType ackValue)
    {
        this.AckValue = ackValue;
    }

    object ITaskResult.AckValue => this.AckValue;

    public TAckType AckValue { get; }
}
