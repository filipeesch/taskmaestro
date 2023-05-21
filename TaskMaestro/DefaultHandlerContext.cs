namespace TaskMaestro;

public class DefaultHandlerContext : IHandlerContext
{
    public Task<TAckValue> GetAckValueByType<TAckValue>(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6>(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5>(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4)> GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4>(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(TAckValue1, TAckValue2, TAckValue3)> GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3>(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(TAckValue1, TAckValue2)> GetAckValuesByTypes<TAckValue1, TAckValue2>(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
