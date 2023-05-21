namespace TaskMaestro.Builders;

public class MaestroBuilder
{
    public ITaskGroupBuilder BuildTaskGroup()
    {
        throw new NotImplementedException();
    }

    public ITaskBuilder BuildTask() => new TaskBuilder<Void, Void>();
}
