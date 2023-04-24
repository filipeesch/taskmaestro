public interface IMaestro
{
    ITaskGroupBuilder BuildTaskGroup();

    ITaskBuilder BuildTask();

    Task FlushAsync(CancellationToken? cancellationToken = null);

    Task RegisterAckAsync<TAckValue>(IAckCode code, TAckValue value);

    // methods to query tasks

    // methods to cancel tasks
}
