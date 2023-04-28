using TaskMaestro;

public interface ISyncTaskBuilder<TIn, TOut>
{
    ITask Create();
}
