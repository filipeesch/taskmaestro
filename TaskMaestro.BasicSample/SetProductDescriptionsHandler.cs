namespace TaskMaestro.BasicSample;

public class SetProductDescriptionsHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<SyncTaskResult<Void>> HandleAsync(ProductInputData input, IHandlerContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Descriptions saved");

        return Task.FromResult(new SyncTaskResult<Void>(Void.Value));
    }
}
