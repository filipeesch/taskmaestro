namespace TaskMaestro.DataStore.SqlServer;

using System.Runtime.Serialization;

public class TaskSqlType
{
    [DataMember(Order = 0)]
    public byte[] Id { get; set; }

    [DataMember(Order = 1)]
    public byte Type { get; set; }

    [DataMember(Order = 2)]
    public byte[] Input { get; set; }

    [DataMember(Order = 3)]
    public string InputType { get; set; }

    [DataMember(Order = 4)]
    public byte[] AckCode { get; set; }

    [DataMember(Order = 5)]
    public string AckValueType { get; set; }

    [DataMember(Order = 6)]
    public byte[]? GroupId { get; set; }

    [DataMember(Order = 7)]
    public string HandlerType { get; set; }

    [DataMember(Order = 8)]
    public DateTime CreatedAt { get; set; }
}
