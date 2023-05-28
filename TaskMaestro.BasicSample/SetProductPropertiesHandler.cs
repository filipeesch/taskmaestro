namespace TaskMaestro.BasicSample;

public class SetProductPropertiesHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public async Task<SyncTaskResult<Void>> HandleAsync(
        ProductInputData input,
        IHandlerContext context,
        CancellationToken cancellationToken)
    {
        var productCreatedAck = await context.GetAckValueByType<ProductCreatedAck>(cancellationToken);
        Console.WriteLine($"Properties saved for product {productCreatedAck.ProductName}");

        return new SyncTaskResult<Void>(Void.Value);
    }
}
