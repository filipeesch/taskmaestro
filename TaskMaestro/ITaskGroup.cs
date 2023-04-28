using TaskMaestro;

public interface ITaskGroup : ITask
{
    void Close();

    // options here, like max parallel tasks
}