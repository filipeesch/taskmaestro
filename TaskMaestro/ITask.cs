public interface ITask
{
    string Id { get; }

    IAckCode AckCode { get; }
}

public interface ITask<TIn, TOut> : ITask
{
}
