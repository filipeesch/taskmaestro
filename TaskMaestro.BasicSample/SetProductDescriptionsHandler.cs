public class SetProductDescriptionsHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<TaskResult<Void>> HandleAsync(ProductInputData input, IHandlerContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}