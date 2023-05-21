using Microsoft.Extensions.DependencyInjection;
using TaskMaestro;
using TaskMaestro.BasicSample;
using TaskMaestro.Builders;
using TaskMaestro.DataStore.SqlServer;

var dataStore = new SqlServerDataStore("Server=localhost;Database=TaskMaestroSample;User Id=sa;Password=Product!2021;");

var services = new ServiceCollection();

services.AddTransient<CreateProductCommandHandler>();
services.AddTransient<SetProductDescriptionsHandler>();
services.AddTransient<SetProductPropertiesHandler>();

var serviceProvider = services.BuildServiceProvider();

var maestro = new MaestroManager(dataStore);
var workersFactory = new WorkerFactory(dataStore, new TaskExecutor(maestro, serviceProvider));
var server = new MaestroServer(workersFactory, new[] { new MaestroQueue("default", 10) });
var builder = new MaestroBuilder();

await server.StartAsync();

while (true)
{
    var userInput = Console.ReadLine();

    if (userInput == "exit")
    {
        break;
    }

    if (userInput.StartsWith("create "))
    {
        // var saveProductGroup = maestro
        //     .BuildTaskGroup()
        //     .Create();

        var productInput = new ProductInputData(userInput.Replace("create ", ""));

        var productTask = builder
            .BuildTask()
            // .InGroup(saveProductGroup)
            .Input(productInput)
            .Produces<ProductCreatedAck>()
            .Async<CreateProductCommandHandler>()
            // .EndsWith(AckCode.FromGuid(correlationId))
            .Create();

        var descriptionsTask = builder
            .BuildTask()
            // .InGroup(saveProductGroup)
            .Input(productInput)
            .WaitFor(productTask.AckCode)
            .Sync<SetProductDescriptionsHandler>()
            .Create();

        var propertiesTask = builder
            .BuildTask()
            // .InGroup(saveProductGroup)
            .Input(productInput)
            .WaitFor(productTask.AckCode)
            .Sync<SetProductPropertiesHandler>()
            .Create();

        // saveProductGroup.Close();

        await maestro.SaveAsync(new[] { productTask, descriptionsTask, propertiesTask });
    }
}

await server.StopAsync();
