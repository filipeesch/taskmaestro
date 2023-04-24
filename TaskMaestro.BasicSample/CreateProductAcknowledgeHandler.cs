public class CreateProductAcknowledgeHandler : IAsyncTaskAckHandler<ProductDocument, ProductDocument>
{
    public Task<TaskResult<ProductDocument>> HandleEndAsync(ProductDocument acknowledge, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
