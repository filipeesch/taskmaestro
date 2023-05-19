namespace TaskMaestro;

public interface ITaskExecutor
{
    Task ExecuteAsync(ITask task, IHandlerContext context, CancellationToken cancellationToken);
}
