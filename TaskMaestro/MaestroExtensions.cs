namespace TaskMaestro;

public static class MaestroExtensions
{
    public static Task SaveAsync(
        this IMaestroManager manager,
        ITask task,
        CancellationToken cancellationToken = default) => manager.SaveAsync(new[] { task }, cancellationToken);
}
