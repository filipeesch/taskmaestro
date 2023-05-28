namespace TaskMaestro;

using System.Buffers.Binary;

internal static class GuidGenerator
{
    private static readonly Random RandomGenerator = new();
    private static uint count = 0;

    public static Guid New()
    {
        var time = DateTime.UtcNow.Ticks;
        var c = Interlocked.Increment(ref count);
        var random = RandomGenerator.Next();

        Span<byte> buffer = stackalloc byte[16];

        BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(0, 8), time);
        BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(8, 4), c);
        BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(12, 4), random);

        return new Guid(buffer);
    }
}
