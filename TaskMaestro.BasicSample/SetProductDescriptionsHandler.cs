public class SetProductDescriptionsHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<TaskResult<Void>> HandleAsync(ProductInputData input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}