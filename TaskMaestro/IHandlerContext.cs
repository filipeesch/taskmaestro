namespace TaskMaestro;

public interface IHandlerContext
{
    Task<TAckValue> GetAckValueByType<TAckValue>(CancellationToken cancellationToken = default);

    Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6>(
            CancellationToken cancellationToken = default);

    Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5>(CancellationToken cancellationToken = default);

    Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4>(CancellationToken cancellationToken = default);

    Task<(TAckValue1, TAckValue2, TAckValue3)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3>(CancellationToken cancellationToken = default);

    Task<(TAckValue1, TAckValue2)>
        GetAckValuesByTypes<TAckValue1, TAckValue2>(CancellationToken cancellationToken = default);
}
