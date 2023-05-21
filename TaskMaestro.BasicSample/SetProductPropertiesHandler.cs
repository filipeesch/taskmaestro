namespace TaskMaestro.BasicSample;

public class SetProductPropertiesHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<SyncTaskResult<Void>> HandleAsync(ProductInputData input, IHandlerContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Properties saved");

        return Task.FromResult(new SyncTaskResult<Void>(Void.Value));
    }
}
