namespace TaskMaestro.DataStore.SqlServer;

using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

public class SqlServerDataStore : IMaestroDataStore
{
    private readonly string connectionString;

    public SqlServerDataStore(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task SaveTasksAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(this.connectionString);

        await connection.OpenAsync(cancellationToken);

        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await SaveTasksAsync(tasks, connection, transaction, cancellationToken);

            await transaction.CommitAsync(CancellationToken.None);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    private static async Task SaveTasksAsync(
        IReadOnlyCollection<ITask> tasks,
        SqlConnection connection,
        SqlTransaction transaction,
        CancellationToken cancellationToken)
    {
        await using var cmd = new SqlCommand(
            @"
INSERT maestro.Tasks (Id, [Type], [Input], InputType, AckCode, AckValueType, GroupId, HandlerType, CreatedAt)
SELECT Id, [Type], [Input], InputType, AckCode, AckValueType, GroupId, HandlerType, CreatedAt FROM @Tasks;

INSERT maestro.TaskWaitAcks (TaskId, Code)
SELECT TaskId, Code FROM @TasksWaitAcks;

INSERT maestro.TaskCompleteAcks (TaskId, Code)
SELECT TaskId, Code FROM @TasksCompleteAcks;
",
            connection,
            transaction);

        cmd.Parameters.Add(
            new SqlParameter(
                "Tasks",
                tasks
                    .OfType<MaestroTask>()
                    .Select(x => MapTaskSqlType(x))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.TaskType",
            });

        cmd.Parameters.Add(
            new SqlParameter(
                "TasksWaitAcks",
                tasks
                    .OfType<MaestroTask>()
                    .SelectMany(task => task.WaitForAcks.Select(ack => (task.Id, ack)))
                    .Select(x => MapTaskAckSqlType(x.Id, x.ack))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.TaskAckType",
            });

        cmd.Parameters.Add(
            new SqlParameter(
                "TasksCompleteAcks",
                tasks
                    .OfType<MaestroTask>()
                    .SelectMany(task => task.CompleteAcks.Select(ack => (task.Id, ack)))
                    .Select(x => MapTaskAckSqlType(x.Id, x.ack))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.TaskAckType",
            });

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private static TaskAckSqlType MapTaskAckSqlType(Guid taskId, AckCode ackCode)
    {
        return new TaskAckSqlType
        {
            TaskId = taskId.ToByteArray(),
            Code = ackCode.Value
        };
    }

    private static TaskSqlType MapTaskSqlType(MaestroTask task)
    {
        return new TaskSqlType
        {
            Id = task.Id.ToByteArray(),
            Type = (byte)task.Type,
            AckCode = task.AckCode.Value,
            Input = SerializeObject(task.Input),
            InputType = task.Input.GetType().AssemblyQualifiedName,
            GroupId = task.Group?.Id.ToByteArray(),
            AckValueType = task.AckValueType.AssemblyQualifiedName,
            HandlerType = task.HandlerType.AssemblyQualifiedName,
            CreatedAt = task.CreatedAt,
        };
    }

    private static byte[] SerializeObject(object data)
    {
        using var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, data);
        return ms.ToArray();
    }

    public Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ITask> ConsumeTaskAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
