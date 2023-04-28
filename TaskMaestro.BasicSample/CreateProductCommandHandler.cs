public class CreateProductCommandHandler : IAsyncTaskHandler<ProductInputData, ProductCreatedAck>
{
    public Task HandleBeginAsync(ProductInputData input, IHandlerContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ProductCreatedAck> HandleEndAsync(IHandlerContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
