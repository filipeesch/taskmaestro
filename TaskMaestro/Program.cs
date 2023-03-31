// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


public interface ITaskHandler<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken cancellationToken);
}

public interface IAsyncTaskHandler<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken cancellationToken);
}

public interface ITask
{
    string Id { get; }

    void WaitFor(string taskId);
}

public interface ITaskManager
{
    Task StartAsync(string taskId);
}
