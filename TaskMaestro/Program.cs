using System.Linq.Expressions;

var maestro = new TaskMaestro();

var saveProductGroup = maestro.CreateTaskGroup();

var correlationId = Guid.NewGuid();
var productInput = new ProductInputData(correlationId);

var productTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .Produces<ProductCreatedAck>()
    .Async<DispatchCreateProductCommandBeginHandler>()
    .WithEndHandler<ProductCreatedEndHandler>()
    .EndsWith(AckCode.FromGuid(correlationId))
    .Create();

var descriptionsTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode, AckCode.FromGuid(correlationId))
    .Sync<SetProductDescriptionsHandler>()
    .Create();

var propertiesTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode)
    .Sync<SetProductPropertiesHandler>()
    .Create();

saveProductGroup.Close();

await maestro.FlushAsync();

public class SetProductDescriptionsHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<TaskResult<Void>> HandleAsync(ProductInputData input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class SetProductPropertiesHandler
    : ISyncTaskHandler<ProductInputData, Void>
{
    public Task<TaskResult<Void>> HandleAsync(ProductInputData input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class DispatchCreateProductCommandBeginHandler : IAsyncTaskBeginHandler<ProductInputData>
{
    public Task HandleBeginAsync(ProductInputData input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class CreateProductAcknowledgeHandler : IAsyncTaskAckHandler<ProductDocument, ProductDocument>
{
    public Task<TaskResult<ProductDocument>> HandleEndAsync(ProductDocument acknowledge, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public record ProductDocument(Guid CorrelationId);

public record ProductCreatedAck(Guid CorrelationId);

public record ProductionProductDocument(Guid CorrelationId);

public record ProductInputData(Guid CorrelationId);

public class TaskResult<TOutput>
{
}


public interface ISyncTaskHandler<in TInput, TOutput>
{
    Task<TaskResult<TOutput>> HandleAsync(TInput input, CancellationToken cancellationToken);
}

public interface IAsyncTaskBeginHandler<in TInput>
{
    Task HandleBeginAsync(TInput input, CancellationToken cancellationToken);
}

public interface IAsyncTaskEndHandler<TAckValue>
{
    Task<TAckValue> HandleEndAsync(CancellationToken cancellationToken);
}

public interface IAsyncTaskAckHandler<in TAck, TOutput>
{
    Task<TaskResult<TOutput>> HandleEndAsync(TAck acknowledge, CancellationToken cancellationToken);
}

public interface IAsyncTaskErrorHandler<in TAcknowledge>
{
    Task HandleErrorAsync(TAcknowledge acknowledge, CancellationToken cancellationToken);
}

public interface ITask
{
    string Id { get; }

    IAckCode AckCode { get; }
}

public interface ITask<TIn, TOut> : ITask
{
}

public interface ITaskBuilder
{
    ITaskBuilder InGroup(ITaskGroup? taskGroup);

    ITaskBuilder<TBuilderInput> BeginAfter<TIn, TOut, TBuilderInput>(
        ITask<TIn, TOut> task,
        Expression<Func<TOut, TBuilderInput>> inputFactory);

    ITaskBuilder<TIn> Input<TIn>(TIn input);

    // options here, like retries

    // metadata here, like headers
}

public interface IAckCode
{
    byte[] Value { get; }
}

public class AckCode : IAckCode
{
    public byte[] Value { get; }

    public static IAckCode FromGuid(Guid value) => null;
}

public interface ITaskBuilder<TIn>
{
    ITaskBuilder<TIn> WaitFor(params IAckCode[] code);

    ITaskBuilder<TIn, TOut> Produces<TOut>();

    ISyncTaskBuilder<TIn, Void, THandler> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, Void>;

    IAsyncTaskBuilder<TIn, Void, TBeginHandler> Async<TBeginHandler>()
        where TBeginHandler : IAsyncTaskBeginHandler<TIn>;
}

public interface ITaskBuilder<TIn, TOut>
{
    ISyncTaskBuilder<TIn, TOut, THandler> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, TOut>;

    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> Async<TBeginHandler>()
        where TBeginHandler : IAsyncTaskBeginHandler<TIn>;
}

public interface ISyncTaskBuilder<TIn, TOut, THandler>
{
    ITask<TIn, TOut> Create();
}

public interface ITaskGroup : ITask
{
    void Close();

    // options here, like max parallel tasks
}

public interface IAsyncTaskBuilder<TIn, TOut, TBeginHandler, TEndHandler>
{
    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> EndsWith(IAckCode code);

    // IAsyncTaskBuilder<TInput, TAck, TOutput> WithErrorHandler<THandler>() where THandler : IAsyncTaskErrorHandler<TAck>;

    ITask<TIn, TOut> Create();
}

public interface IAsyncTaskBuilder<TIn, TOut, TBeginHandler>
{
    IAsyncTaskBuilder<TIn, TOut, TBeginHandler, TEndHandler> WithEndHandler<TEndHandler>()
        where TEndHandler : IAsyncTaskEndHandler<TOut>;

    IAsyncTaskBuilder<TIn, TOut, TBeginHandler> EndsWith(IAckCode code);

    // IAsyncTaskBuilder<TInput, TAck, TOutput> WithErrorHandler<THandler>() where THandler : IAsyncTaskErrorHandler<TAck>;

    ITask<TIn, TOut> Create();
}

public interface ITaskMaestro
{
    ITaskGroup CreateTaskGroup(ITaskGroup? taskGroup = null);

    ITaskBuilder BuildTask();

    Task FlushAsync(CancellationToken? cancellationToken = null);

    Task AckAsyncTaskAsync<TAcknowledge>(TAcknowledge acknowledge);

    // methods to query tasks

    // methods to cancel tasks
}

public class TaskMaestro : ITaskMaestro
{
    public ITaskGroup CreateTaskGroup(ITaskGroup? taskGroup = null)
    {
        throw new NotImplementedException();
    }

    public ITaskBuilder BuildTask()
    {
        throw new NotImplementedException();
    }

    public Task FlushAsync(CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task AckAsyncTaskAsync<TAcknowledge>(TAcknowledge acknowledge)
    {
        throw new NotImplementedException();
    }
}


public class Void
{
    public static Void Value = new Void();

    private Void()
    {
    }
}

public class ProductCreatedEndHandler : IAsyncTaskEndHandler<ProductCreatedAck>
{
    public Task<ProductCreatedAck> HandleEndAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
