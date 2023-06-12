namespace TaskMaestro.DataStore.SqlServer;

using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.IO;
using TaskMaestro.DataStore.SqlServer.SqlTypes;

public class SqlServerDataStore : IMaestroDataStore
{
    private readonly Func<SqlConnection> connectionFactory;
    private static readonly RecyclableMemoryStreamManager MemoryStreamManager = new(1024, 1024, 128 * 1024);

    public SqlServerDataStore(Func<SqlConnection> connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    public async Task SaveTasksAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken = default)
    {
        await using var connection = this.connectionFactory();

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
INSERT maestro.Tasks (Id, [Type], [Input], InputType, AckCode, AckValueType, GroupId, HandlerType, Queue, CreatedAt, Status, MaxRetryCount, CurrentRetryCount)
SELECT Id, [Type], [Input], InputType, AckCode, AckValueType, GroupId, HandlerType, Queue, CreatedAt, Status, MaxRetryCount, CurrentRetryCount FROM @Tasks;

INSERT maestro.TaskAcks (TaskId, Code)
SELECT TaskId, Code FROM @TasksAcks;
",
            connection,
            transaction);

        cmd.Parameters.Add(
            new SqlParameter(
                "Tasks",
                tasks
                    .Select(x => ToTaskSqlType(x))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.TaskType",
            });

        cmd.Parameters.Add(
            new SqlParameter(
                "TasksAcks",
                tasks
                    .SelectMany(x => ToTaskAckSqlType(x))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.TaskAckType",
            });

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private static IEnumerable<TaskAckSqlType> ToTaskAckSqlType(ITask task)
    {
        var taskId = task.Id.ToByteArray();

        foreach (var ack in task.WaitForAcks)
        {
            yield return new TaskAckSqlType
            {
                Code = ack.Value,
                TaskId = taskId
            };
        }
    }

    private static TaskSqlType ToTaskSqlType(ITask task)
    {
        return task switch
        {
            SyncTask syncTask => new TaskSqlType
            {
                Id = syncTask.Id.ToByteArray(),
                Type = (byte)TaskType.Sync,
                AckCode = syncTask.AckCode.Value,
                Input = SerializeObject(syncTask.Input),
                InputType = syncTask.Input.GetType().AssemblyQualifiedName,
                GroupId = syncTask.GroupId?.ToByteArray(),
                AckValueType = syncTask.AckValueType.AssemblyQualifiedName,
                HandlerType = syncTask.HandlerType.AssemblyQualifiedName,
                Queue = syncTask.Queue,
                CreatedAt = syncTask.CreatedAt,
            },
            AsyncBeginTask asyncBeginTask => new TaskSqlType
            {
                Id = asyncBeginTask.Id.ToByteArray(),
                Type = (byte)TaskType.AsyncBegin,
                AckCode = asyncBeginTask.AckCode.Value,
                Input = SerializeObject(asyncBeginTask.Input),
                InputType = asyncBeginTask.Input.GetType().AssemblyQualifiedName,
                GroupId = asyncBeginTask.GroupId?.ToByteArray(),
                AckValueType = asyncBeginTask.AckValueType.AssemblyQualifiedName,
                HandlerType = asyncBeginTask.HandlerType.AssemblyQualifiedName,
                Queue = asyncBeginTask.Queue,
                CreatedAt = asyncBeginTask.CreatedAt,
            },
            AsyncEndTask asyncEndTask => new TaskSqlType
            {
                Id = asyncEndTask.Id.ToByteArray(),
                Type = (byte)TaskType.AsyncEnd,
                AckCode = asyncEndTask.AckCode.Value,
                Input = SerializeObject(asyncEndTask.Input),
                InputType = asyncEndTask.Input.GetType().AssemblyQualifiedName,
                GroupId = null,
                AckValueType = asyncEndTask.AckValueType.AssemblyQualifiedName,
                HandlerType = asyncEndTask.HandlerType.AssemblyQualifiedName,
                Queue = asyncEndTask.Queue,
                CreatedAt = asyncEndTask.CreatedAt,
            },
            _ => throw new InvalidOperationException("Task type is not supported")
        };
    }

    private static byte[] SerializeObject(object data)
    {
        using var ms = MemoryStreamManager.GetStream();
        JsonSerializer.Serialize(ms, data);
        return ms.ToArray();
    }

    private static object? DeserializeObject(byte[]? data, Type type)
    {
        if (data is null)
        {
            return null;
        }

        if (data.Length == 0)
        {
            return new object();
        }

        return JsonSerializer.Deserialize(data, type);
    }

    public async Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken cancellationToken = default)
    {
        await using var connection = this.connectionFactory();

        await using var cmd = new SqlCommand(
            @"
INSERT maestro.Acks (Code, [Value], ValueType, CreatedAt)
SELECT Code, [Value], ValueType, CreatedAt FROM @Acks;
",
            connection);

        cmd.Parameters.Add(
            new SqlParameter(
                "Acks",
                acks
                    .Select(x => ToAckSqlType(x))
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.AckType",
            });

        await connection.OpenAsync(cancellationToken);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task CompleteTaskAsync(TaskExecutionReport report, CancellationToken cancellationToken)
    {
        await using var connection = this.connectionFactory();

        await connection.OpenAsync(cancellationToken);

        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await using var command = new SqlCommand(
                @"
INSERT maestro.TaskExecutionReports (TaskId, [Type], Message, CreatedAt)
VALUES(@TaskId, @Type, @Message, @CreatedAt);

UPDATE maestro.Tasks
SET
    CompletedAt = @CreatedAt,
    FetchedAt = IIF(CurrentRetryCount <  MaxRetryCount, NULL, FetchedAt),
    CurrentRetryCount = IIF(CurrentRetryCount <  MaxRetryCount, CurrentRetryCount + 1, MaxRetryCount),
    Status =
        CASE
            WHEN @Type = 1 THEN 3 --Completed
            WHEN CurrentRetryCount <  MaxRetryCount THEN 5 --Retry
            WHEN CurrentRetryCount = MaxRetryCount THEN 4 --Error
        END
WHERE Id = @TaskId
",
                connection,
                transaction);

            command.Parameters.AddWithValue("TaskId", report.TaskId.ToByteArray());
            command.Parameters.AddWithValue("Type", (byte)report.Type);
            command.Parameters.AddWithValue("Message", report.Message);
            command.Parameters.AddWithValue("CreatedAt", report.CreatedAt);

            await command.ExecuteNonQueryAsync(cancellationToken);

            await transaction.CommitAsync(CancellationToken.None);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    private AckSqlType ToAckSqlType(Ack ack)
    {
        return new AckSqlType
        {
            Code = ack.Code.Value,
            Value = SerializeObject(ack.Value),
            ValueType = ack.Value.GetType().AssemblyQualifiedName,
            CreatedAt = ack.CreatedAt,
        };
    }

    public async IAsyncEnumerable<ITask> ConsumeTasksAsync(string queue, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (true)
            {
                var task = await this.TryFetchTaskAsync(queue, cancellationToken);

                if (task is null)
                {
                    break;
                }

                yield return ToMaestroTask(task);
            }

            try
            {
                await Task.Delay(200, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    public async IAsyncEnumerable<object> GetAckValueByTypesAsync(
        Guid taskId,
        IEnumerable<Type> types,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var connection = this.connectionFactory();

        #region SQL

        const string sql = @"
SELECT a.ValueType, a.Value
FROM @AcksTypes t
	INNER JOIN maestro.Acks a ON a.ValueType = t.ValueType
WHERE EXISTS(SELECT * FROM maestro.TaskAcks ta WHERE ta.Code = a.Code AND ta.TaskId = @TaskId)
";

        #endregion

        var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("TaskId", taskId.ToByteArray()));

        command.Parameters.Add(
            new SqlParameter(
                "AcksTypes",
                types
                    .Select(
                        t => new AckSqlType
                        {
                            ValueType = t.AssemblyQualifiedName,
                            Code = Array.Empty<byte>(),
                            Value = Array.Empty<byte>(),
                            CreatedAt = DateTime.MinValue,
                        })
                    .ToDataReader())
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "maestro.AckType",
            });

        await connection.OpenAsync(cancellationToken);

        await using var dr = await command.ExecuteReaderAsync(cancellationToken);

        while (await dr.ReadAsync(cancellationToken))
        {
            var typeName = dr.GetString(0);

            var type = Type.GetType(typeName);

            if (type is null)
            {
                throw new InvalidOperationException($"The type '{typeName}' was not found in the application context");
            }

            var data = dr.GetSqlBinary(1).Value;

            yield return DeserializeObject(data, type)!;
        }
    }

    private static ITask ToMaestroTask(TaskSqlType task)
    {
        return (TaskType)task.Type switch
        {
            TaskType.Sync => new SyncTask(
                new Guid(task.Id),
                new AckCode(task.AckCode),
                DeserializeObject(task.Input, Type.GetType(task.InputType)),
                Type.GetType(task.AckValueType),
                task.GroupId is null ? null : new Guid(task.GroupId),
                Array.Empty<AckCode>(), // TODO
                Type.GetType(task.HandlerType),
                task.Queue,
                task.CreatedAt,
                task.FetchedAt,
                task.CompletedAt,
                (TaskStatus)task.Status,
                task.MaxRetryCount,
                task.CurrentRetryCount),
            TaskType.AsyncBegin => new AsyncBeginTask(
                new Guid(task.Id),
                new AckCode(task.AckCode),
                DeserializeObject(task.Input, Type.GetType(task.InputType)),
                Type.GetType(task.AckValueType),
                task.GroupId is null ? null : new Guid(task.GroupId),
                Array.Empty<AckCode>(), // TODO
                Type.GetType(task.HandlerType),
                task.Queue,
                task.CreatedAt,
                task.FetchedAt,
                task.CompletedAt,
                (TaskStatus)task.Status,
                task.MaxRetryCount,
                task.CurrentRetryCount),
            TaskType.AsyncEnd => new AsyncEndTask(
                new Guid(task.Id),
                new AckCode(task.AckCode),
                Type.GetType(task.AckValueType),
                DeserializeObject(task.Input, Type.GetType(task.InputType)),
                Type.GetType(task.InputType),
                Array.Empty<AckCode>(), // TODO
                Type.GetType(task.HandlerType),
                task.Queue,
                task.CreatedAt,
                task.FetchedAt,
                task.CompletedAt,
                (TaskStatus)task.Status,
                task.MaxRetryCount,
                task.CurrentRetryCount),
            _ => throw new InvalidOperationException()
        };
    }

    private async Task<TaskSqlType?> TryFetchTaskAsync(string queue, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = this.connectionFactory();

            await connection.OpenAsync(cancellationToken);

            #region SQL

            const string sql = @"
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET TRAN ISOLATION LEVEL READ COMMITTED;

UPDATE maestro.Tasks
SET FetchedAt = GETUTCDATE(), Status = @Status
OUTPUT 
    inserted.Id,
    inserted.[Type],
    inserted.[Input],
    inserted.InputType,
    inserted.AckCode,
    inserted.AckValueType,
    inserted.GroupId,
    inserted.HandlerType,
    inserted.Queue,
    inserted.CreatedAt,
    inserted.FetchedAt,
    inserted.CompletedAt,
    inserted.Status,
    inserted.MaxRetryCount,
    inserted.CurrentRetryCount
WHERE Id IN(
	SELECT TOP(1) t.Id FROM maestro.Tasks t WITH (READPAST, UPDLOCK, ROWLOCK)
	WHERE
		t.Queue = @Queue AND
		t.FetchedAt IS NULL AND
		(
			SELECT COUNT(*)
			FROM maestro.TaskAcks ta
			WHERE
				ta.TaskId = t.Id AND
				NOT EXISTS(
					SELECT * FROM maestro.Acks a
					WHERE a.Code = ta.Code)
		) = 0
	ORDER BY t.CreatedAt
)
";

            #endregion

            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("Status", (byte)TaskStatus.Fetched);
            command.Parameters.AddWithValue("Queue", queue);

            await using var dr = await command.ExecuteReaderAsync(CancellationToken.None);

            if (!await dr.ReadAsync(CancellationToken.None))
            {
                return null;
            }

            return new TaskSqlType
            {
                Id = dr.GetSqlBinary(0).Value,
                Type = dr.GetByte(1),
                Input = dr.GetSqlBinary(2).Value,
                InputType = dr.GetString(3),
                AckCode = dr.GetSqlBinary(4).Value,
                AckValueType = dr.GetString(5),
                GroupId = dr.GetNullable<byte[]>(6),
                HandlerType = dr.GetString(7),
                Queue = dr.GetString(8),
                CreatedAt = dr.GetDateTime(9),
                FetchedAt = dr.GetNullable<DateTime?>(10),
                CompletedAt = dr.GetNullable<DateTime?>(11),
                Status = dr.GetByte(12),
                MaxRetryCount = dr.GetInt32(13),
                CurrentRetryCount = dr.GetInt32(14),
            };
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }
}
