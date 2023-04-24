public interface ISyncTaskBuilder<TIn, TOut, THandler>
{
    ITask<TIn, TOut> Create();
}