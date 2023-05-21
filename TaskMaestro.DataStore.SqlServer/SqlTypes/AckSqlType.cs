namespace TaskMaestro.DataStore.SqlServer.SqlTypes;

using System.Runtime.Serialization;

public class AckSqlType
{
    [DataMember(Order = 0)]
    public byte[] Code { get; set; }

    [DataMember(Order = 1)]
    public byte[] Value { get; set; }

    [DataMember(Order = 2)]
    public string ValueType { get; set; }

    [DataMember(Order = 3)]
    public DateTime CreatedAt { get; set; }
}
