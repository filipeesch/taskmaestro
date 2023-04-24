public class DispatchCreateProductCommandBeginHandler : IAsyncTaskBeginHandler<ProductInputData>
{
    public Task HandleBeginAsync(ProductInputData input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}