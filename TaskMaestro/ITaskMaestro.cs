public interface ITaskGroupBuilder : ITaskGroupInGroupBuilder
{
    ITaskGroupInGroupBuilder InGroup(ITaskGroup? taskGroup);
}
