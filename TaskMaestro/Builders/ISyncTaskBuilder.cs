namespace TaskMaestro.Builders;

public interface ISyncTaskBuilder<TIn, TOut>
{
    ITask Create();
}