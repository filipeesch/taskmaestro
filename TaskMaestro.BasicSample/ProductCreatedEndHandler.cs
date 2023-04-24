public class ProductCreatedEndHandler : IAsyncTaskEndHandler<ProductCreatedAck>
{
    public Task<ProductCreatedAck> HandleEndAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}