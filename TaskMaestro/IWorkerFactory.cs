namespace TaskMaestro;

public interface IWorkerFactory
{
    IMaestroWorker Create(string queueName);
}
