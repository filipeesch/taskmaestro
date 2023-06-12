namespace TaskMaestro;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskMaestro.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMaestro(this IServiceCollection services, Action<MaestroConfigurationBuilder> builder)
    {
        var builderInstance = new MaestroConfigurationBuilder();

        builder(builderInstance);

        return services
            .AddSingleton<IHostedService, MaestroHostedService>()
            .AddSingleton<IMaestroManager, MaestroManager>()
            .AddSingleton<IMaestroDataStore>(provider => builderInstance.DataStoreFactory(provider))
            .AddSingleton<ITaskExecutor, TaskExecutor>()
            .AddSingleton<IWorkerFactory, WorkerFactory>()
            .AddSingleton<IMaestroServer>(
                provider => new MaestroServer(
                    provider.GetRequiredService<IWorkerFactory>(),
                    builderInstance.Queues));
    }
}
