namespace TaskMaestro;

public interface IMaestroServer : IDisposable
{
    Task StartAsync();

    Task StopAsync();
}
