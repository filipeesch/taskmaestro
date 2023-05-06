using TaskMaestro;
using TaskMaestro.DataStore.SqlServer;

var maestro = new Maestro(new SqlServerDataStore("Server=localhost;Database=TaskMaestroSample;User Id=sa;Password=Product!2021;"));

// var saveProductGroup = maestro
//     .BuildTaskGroup()
//     .Create();

var correlationId = Guid.NewGuid();
var productInput = new ProductInputData(correlationId);

var productTask = maestro
    .BuildTask()
    // .InGroup(saveProductGroup)
    .Input(productInput)
    .Produces<ProductCreatedAck>()
    .Async<CreateProductCommandHandler>()
    .EndsWith(AckCode.FromGuid(correlationId))
    .Create();

var descriptionsTask = maestro
    .BuildTask()
    // .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode, AckCode.FromGuid(correlationId))
    .Sync<SetProductDescriptionsHandler>()
    .Create();

var propertiesTask = maestro
    .BuildTask()
    // .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode)
    .Sync<SetProductPropertiesHandler>()
    .Create();

// saveProductGroup.Close();

await maestro.SaveAsync(new[] { productTask, descriptionsTask, propertiesTask });
