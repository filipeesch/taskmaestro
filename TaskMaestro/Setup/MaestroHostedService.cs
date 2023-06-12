namespace TaskMaestro.Setup;

using Microsoft.Extensions.Hosting;

internal class MaestroHostedService : IHostedService
{
    private readonly IMaestroServer server;

    public MaestroHostedService(IMaestroServer server)
    {
        this.server = server;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await this.server.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await this.server.StopAsync();
    }
}
