namespace TaskMaestro;

public class Ack
{
    public Ack(AckCode code, object value)
    {
        this.Code = code;
        this.Value = value;
        this.CreatedAt = DateTime.UtcNow;
    }

    public AckCode Code { get; }

    public object Value { get; }

    public DateTime CreatedAt { get; }
}
