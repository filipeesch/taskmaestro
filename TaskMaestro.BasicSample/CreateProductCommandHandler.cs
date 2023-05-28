namespace TaskMaestro.BasicSample;

public class CreateProductCommandHandler : IAsyncTaskHandler<ProductInputData, ProductCreatedAck>
{
    public Task<AsyncBeginTaskResult> HandleBeginAsync(ProductInputData input, IHandlerContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Saving product: " + input.Name);

        return Task.FromResult(new AsyncBeginTaskResult());
    }

    public Task<AsyncEndTaskResult<ProductCreatedAck>> HandleEndAsync(
        ProductInputData input,
        IHandlerContext context,
        CancellationToken cancellationToken)
    {
        var productId = new Random().Next(1000, 9999);
        Console.WriteLine($"Product saved: Id={productId}, Name={input.Name}");

        return Task.FromResult(new AsyncEndTaskResult<ProductCreatedAck>(new ProductCreatedAck(productId, input.Name)));
    }
}
