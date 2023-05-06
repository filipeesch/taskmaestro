namespace TaskMaestro.DataStore.SqlServer;

using System.Runtime.Serialization;

public class TaskAckSqlType
{
    [DataMember(Order = 0)]
    public byte[] TaskId { get; set; }

    [DataMember(Order = 1)]
    public byte[] Code { get; set; }
}