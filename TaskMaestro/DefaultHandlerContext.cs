namespace TaskMaestro;

using System.Linq;

public class DefaultHandlerContext : IHandlerContext
{
    private readonly IMaestroDataStore dataStore;
    private readonly ITask task;

    public DefaultHandlerContext(IMaestroDataStore dataStore, ITask task)
    {
        this.dataStore = dataStore;
        this.task = task;
    }

    public async Task<TAckValue> GetAckValueByType<TAckValue>(CancellationToken cancellationToken = default)
    {
        var result = await this.GetAckValuesByTypes<TAckValue, Void, Void, Void, Void, Void>(cancellationToken);
        return result.Item1;
    }

    public async Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5>(CancellationToken cancellationToken = default)
    {
        var result = await this.GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, Void>(cancellationToken);

        return (result.Item1, result.Item2, result.Item3, result.Item4, result.Item5);
    }

    public async Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4)> GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4>(
        CancellationToken cancellationToken = default)
    {
        var result =
            await this.GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, Void, Void>(cancellationToken);

        return (result.Item1, result.Item2, result.Item3, result.Item4);
    }

    public async Task<(TAckValue1, TAckValue2, TAckValue3)> GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3>(
        CancellationToken cancellationToken = default)
    {
        var result = await this.GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, Void, Void, Void>(cancellationToken);

        return (result.Item1, result.Item2, result.Item3);
    }

    public async Task<(TAckValue1, TAckValue2)> GetAckValuesByTypes<TAckValue1, TAckValue2>(CancellationToken cancellationToken = default)
    {
        var result = await this.GetAckValuesByTypes<TAckValue1, TAckValue2, Void, Void, Void, Void>(cancellationToken);

        return (result.Item1, result.Item2);
    }

    public async Task<(TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6)>
        GetAckValuesByTypes<TAckValue1, TAckValue2, TAckValue3, TAckValue4, TAckValue5, TAckValue6>(
            CancellationToken cancellationToken = default)
    {
        var types = new[]
            {
                typeof(TAckValue1),
                typeof(TAckValue2),
                typeof(TAckValue3),
                typeof(TAckValue4),
                typeof(TAckValue5),
                typeof(TAckValue6),
            }
            .Where(t => t != typeof(Void));

        var values = await this.dataStore
            .GetAckValueByTypesAsync(this.task.Id, types, cancellationToken)
            .ToListAsync(cancellationToken);

        return
        (
            (TAckValue1)(values.ElementAtOrDefault(0) ?? Void.Value),
            (TAckValue2)(values.ElementAtOrDefault(1) ?? Void.Value),
            (TAckValue3)(values.ElementAtOrDefault(2) ?? Void.Value),
            (TAckValue4)(values.ElementAtOrDefault(3) ?? Void.Value),
            (TAckValue5)(values.ElementAtOrDefault(4) ?? Void.Value),
            (TAckValue6)(values.ElementAtOrDefault(5) ?? Void.Value)
        );
    }
}
