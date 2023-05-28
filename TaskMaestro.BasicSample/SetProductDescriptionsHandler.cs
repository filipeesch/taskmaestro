namespace TaskMaestro.BasicSample;

public class SetProductDescriptionsHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public async Task<SyncTaskResult<Void>> HandleAsync(
        ProductInputData input,
        IHandlerContext context,
        CancellationToken cancellationToken)
    {
        var productCreatedAck = await context.GetAckValueByType<ProductCreatedAck>(cancellationToken);

        Console.WriteLine($"Descriptions saved for product {productCreatedAck.ProductName}");

        return new SyncTaskResult<Void>(Void.Value);
    }
}
