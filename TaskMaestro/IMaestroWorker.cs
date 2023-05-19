namespace TaskMaestro;

public interface IMaestroWorker : IDisposable
{
    Task StartAsync();

    Task StopAsync();

    void Dispose();
}
