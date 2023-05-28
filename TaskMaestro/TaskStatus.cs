namespace TaskMaestro;

public enum TaskStatus : byte
{
    Created = 1,
    Fetched = 2,
    Completed = 3,
    Error = 4,
    Retry = 5,
}
