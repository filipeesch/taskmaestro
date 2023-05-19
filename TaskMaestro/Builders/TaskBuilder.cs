namespace TaskMaestro.Builders;

internal class TaskBuilder<TIn, TAckValue>
    : ITaskBuilder,
        ITaskBuilder<TIn>,
        ITaskBuilder<TIn, TAckValue>,
        ISyncTaskBuilder<TIn, TAckValue>,
        IAsyncTaskBuilder<TIn, TAckValue>
{
    private TIn? input;

    private ITaskGroup? group;
    private List<AckCode> waitForAcks = new();
    private Type? handlerType;
    private List<AckCode> endAcks = new();

    public ITaskBuilder InGroup(ITaskGroup? group)
    {
        this.group = group;
        return this;
    }

    public ITaskBuilder<TNewIn> Input<TNewIn>(TNewIn input)
    {
        return new TaskBuilder<TNewIn, TAckValue>
        {
            group = this.group,
            input = input,
        };
    }

    public ITaskBuilder<TIn> WaitFor(AckCode code)
    {
        this.waitForAcks.Add(code);
        return this;
    }

    public ITaskBuilder<TIn> WaitFor(params AckCode[] codes)
    {
        this.waitForAcks.AddRange(codes);
        return this;
    }

    public ITaskBuilder<TIn, TNewAckValue> Produces<TNewAckValue>()
    {
        var builder = this.NewBuilderType<TNewAckValue>();
        return builder;
    }

    public ISyncTaskBuilder<TIn, Void> Sync<THandler>() where THandler : ISyncTaskHandler<TIn, Void>
    {
        var builder = this.NewBuilderType<Void>();
        builder.handlerType = typeof(THandler);

        return builder;
    }

    ISyncTaskBuilder<TIn, TAckValue> ITaskBuilder<TIn, TAckValue>.Sync<THandler>()
    {
        this.handlerType = typeof(THandler);
        return this;
    }

    IAsyncTaskBuilder<TIn, TAckValue> ITaskBuilder<TIn, TAckValue>.Async<THandler>()
    {
        this.handlerType = typeof(THandler);
        return this;
    }

    public IAsyncTaskBuilder<TIn, Void> Async<THandler>() where THandler : IAsyncTaskHandler<TIn, Void>
    {
        var builder = this.NewBuilderType<Void>();
        builder.handlerType = typeof(THandler);

        return builder;
    }

    // public IAsyncTaskBuilder<TIn, TAckValue> EndsWith(params AckCode[] codes)
    // {
    //     this.endAcks.AddRange(codes);
    //     return this;
    // }
    //
    // public IAsyncTaskBuilder<TIn, TAckValue> EndsWith(AckCode code)
    // {
    //     this.endAcks.Add(code);
    //     return this;
    // }

    ITask IAsyncTaskBuilder<TIn, TAckValue>.Create()
    {
        return new AsyncBeginTask(
            this.input ?? new object(),
            typeof(TAckValue),
            this.group?.Id,
            this.waitForAcks,
            this.handlerType);
    }

    ITask ISyncTaskBuilder<TIn, TAckValue>.Create()
    {
        return new SyncTask(
            this.input ?? new object(),
            typeof(TAckValue),
            this.group?.Id,
            this.waitForAcks,
            this.handlerType);
    }

    private TaskBuilder<TIn, TNewAck> NewBuilderType<TNewAck>()
    {
        return new TaskBuilder<TIn, TNewAck>
        {
            input = this.input,
            group = this.group,
            handlerType = this.handlerType,
            endAcks = this.endAcks,
            waitForAcks = this.waitForAcks
        };
    }
}
