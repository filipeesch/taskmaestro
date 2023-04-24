public class AckCode : IAckCode
{
    public byte[] Value { get; }

    public static IAckCode FromGuid(Guid value) => null;
}