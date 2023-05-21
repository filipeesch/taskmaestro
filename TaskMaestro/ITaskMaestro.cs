namespace TaskMaestro;

using TaskMaestro.Builders;

public interface ITaskGroupBuilder : ITaskGroupInGroupBuilder
{
    ITaskGroupInGroupBuilder InGroup(ITaskGroup? taskGroup);
}
