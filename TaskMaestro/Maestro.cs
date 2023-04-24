public class Maestro : IMaestro
{
    public ITaskGroupBuilder BuildTaskGroup()
    {
        throw new NotImplementedException();
    }

    public ITaskBuilder BuildTask()
    {
        throw new NotImplementedException();
    }

    public Task FlushAsync(CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task RegisterAckAsync<TAckValue>(IAckCode code, TAckValue value)
    {
        throw new NotImplementedException();
    }
}
