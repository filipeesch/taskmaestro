namespace TaskMaestro;

public class AckCode
{
    public AckCode(byte[] value) => this.Value = value;

    public byte[] Value { get; }

    public static AckCode FromGuid(Guid value) => new(value.ToByteArray());
}
