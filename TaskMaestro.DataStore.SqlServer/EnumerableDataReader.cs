namespace TaskMaestro.DataStore.SqlServer;

using System.Collections;
using System.Data.Common;
using System.Reflection;

internal class EnumerableDataReader<T> : DbDataReader
{
    private readonly IEnumerator<T> source;
    private readonly PropertyInfo[] properties;

    public EnumerableDataReader(IEnumerable<T> source)
    {
        this.source = source.GetEnumerator();
        this.properties = typeof(T).GetProperties();
    }

    public override bool GetBoolean(int ordinal) => throw new NotSupportedException();

    public override byte GetByte(int ordinal) => throw new NotSupportedException();

    public override long GetBytes(
        int ordinal,
        long dataOffset,
        byte[]? buffer,
        int bufferOffset,
        int length) => throw new NotSupportedException();

    public override char GetChar(int ordinal) => throw new NotSupportedException();

    public override long GetChars(
        int ordinal,
        long dataOffset,
        char[]? buffer,
        int bufferOffset,
        int length) => throw new NotSupportedException();

    public override string GetDataTypeName(int ordinal) => throw new NotSupportedException();

    public override DateTime GetDateTime(int ordinal) => throw new NotSupportedException();

    public override decimal GetDecimal(int ordinal) => throw new NotSupportedException();

    public override double GetDouble(int ordinal) => throw new NotSupportedException();

    public override Type GetFieldType(int ordinal) => throw new NotSupportedException();

    public override float GetFloat(int ordinal) => throw new NotSupportedException();

    public override Guid GetGuid(int ordinal) => throw new NotSupportedException();

    public override short GetInt16(int ordinal) => throw new NotSupportedException();

    public override int GetInt32(int ordinal) => throw new NotSupportedException();

    public override long GetInt64(int ordinal) => throw new NotSupportedException();

    public override string GetName(int ordinal) => this.properties[ordinal].Name;

    public override int GetOrdinal(string name) => throw new NotSupportedException();

    public override string GetString(int ordinal) => throw new NotSupportedException();

    public override object GetValue(int ordinal) => this.properties[ordinal].GetValue(this.source.Current)!;

    public override int GetValues(object[] values) => throw new NotSupportedException();

    public override bool IsDBNull(int ordinal) => throw new NotSupportedException();

    public override int FieldCount => this.properties.Length;

    public override object this[int ordinal] => throw new NotSupportedException();

    public override object this[string name] => throw new NotSupportedException();

    public override int RecordsAffected => throw new NotSupportedException();

    public override bool HasRows => throw new NotSupportedException();

    public override bool IsClosed => throw new NotSupportedException();

    public override bool NextResult() => throw new NotSupportedException();

    public override bool Read() => this.source.MoveNext();

    public override int Depth => -1;

    public override IEnumerator GetEnumerator() => throw new NotSupportedException();
}
