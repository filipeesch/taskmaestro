var maestro = new Maestro();

var saveProductGroup = maestro
    .BuildTaskGroup()
    .Create();

var correlationId = Guid.NewGuid();
var productInput = new ProductInputData(correlationId);

var productTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .Produces<ProductCreatedAck>()
    .Async<DispatchCreateProductCommandBeginHandler>()
    .WithEndHandler<ProductCreatedEndHandler>()
    .EndsWith(AckCode.FromGuid(correlationId))
    .Create();

var descriptionsTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode, AckCode.FromGuid(correlationId))
    .Sync<SetProductDescriptionsHandler>()
    .Create();

var propertiesTask = maestro
    .BuildTask()
    .InGroup(saveProductGroup)
    .Input(productInput)
    .WaitFor(productTask.AckCode)
    .Sync<SetProductPropertiesHandler>()
    .Create();

saveProductGroup.Close();

await maestro.FlushAsync();
