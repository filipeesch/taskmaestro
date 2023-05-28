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

await server.StartAsync();

while (true)
{
    var userInput = Console.ReadLine();

    if (userInput == "exit")
    {
        break;
    }

    if (userInput.StartsWith("create many "))
    {
        var inputs = userInput.Split(" ");
        var count = int.Parse(inputs[2]);
        var productName = inputs[3];

        var tasks = new List<ITask>();

        for (var i = 0; i < count; i++)
        {
            tasks.AddRange(CreateTask(productName + " " + i));
        }

        await maestro.SaveAsync(tasks);
        continue;
    }

    if (userInput.StartsWith("create "))
    {
        var tasks = CreateTask(userInput.Replace("create ", ""));

        await maestro.SaveAsync(tasks);
        continue;
    }
}

await server.StopAsync();

IReadOnlyCollection<ITask> CreateTask(string productName)
{
    var builder = new MaestroBuilder();
    // var saveProductGroup = maestro
    //     .BuildTaskGroup()
    //     .Create();

    var productInput = new ProductInputData(productName);

    var productTask = builder
        .BuildTask()
        //.InGroup(saveProductGroup)
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

    return new[] { productTask, descriptionsTask, propertiesTask };
}
