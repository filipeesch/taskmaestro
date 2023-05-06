namespace TaskMaestro.DataStore.SqlServer;

using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

internal class EnumerableDataReader<T> : DbDataReader
{
    private readonly IEnumerator<T> source;
    private static readonly IReadOnlyList<PropertyInfo> Properties;
    private static readonly IReadOnlyList<Func<T, object>> CompiledGetters;
    private static readonly DataTable SchemaTable;

    static EnumerableDataReader()
    {
        Properties = typeof(T)
            .GetProperties()
            .Select(property => (property, property.GetCustomAttribute<DataMemberAttribute>()?.Order))
            .OrderBy(x => x.Order)
            .Select(x => x.property)
            .ToList();

        SchemaTable = CreateSchemaTable(Properties);

        CompiledGetters = Properties
            .Select(
                property =>
                {
                    var arg = Expression.Parameter(typeof(T), "x");
                    var expr = Expression.Convert(Expression.Property(arg, property.Name), typeof(object));

                    return Expression.Lambda<Func<T, object>>(expr, arg).Compile();
                })
            .ToList();
    }

    public EnumerableDataReader(IEnumerable<T> source)
    {
        this.source = source.GetEnumerator();
    }

    public override DataTable? GetSchemaTable() => SchemaTable;

    private static DataTable CreateSchemaTable(IEnumerable<PropertyInfo> properties)
    {
        var dt = new DataTable();

        dt.Columns.Add(SchemaTableColumn.ColumnName, typeof(string));
        dt.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int));
        dt.Columns.Add(SchemaTableColumn.IsKey, typeof(bool));
        dt.Columns.Add(SchemaTableColumn.DataType, typeof(Type));
        dt.Columns.Add(SchemaTableColumn.ProviderType, typeof(int));
        dt.Columns.Add(SchemaTableColumn.ColumnSize, typeof(int));
        dt.Columns.Add(SchemaTableColumn.NumericPrecision, typeof(short));
        dt.Columns.Add(SchemaTableColumn.NumericScale, typeof(short));
        dt.Columns.Add("DataTypeName", typeof(string));
        var propertyOrdinal = 0;

        foreach (var property in properties)
        {
            var row = dt.NewRow();

            row[SchemaTableColumnOrdinal.ColumnName] = property.Name;
            row[SchemaTableColumnOrdinal.ColumnOrdinal] = propertyOrdinal++;
            row[SchemaTableColumnOrdinal.IsKey] = DBNull.Value;
            row[SchemaTableColumnOrdinal.DataType] = property.PropertyType;
            row[SchemaTableColumnOrdinal.ColumnSize] = DBNull.Value;
            row[SchemaTableColumnOrdinal.NumericPrecision] = DBNull.Value;
            row[SchemaTableColumnOrdinal.NumericScale] = DBNull.Value;

            if (property.PropertyType == typeof(int))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.Int;
                row[SchemaTableColumnOrdinal.DataTypeName] = "Int";
            }
            else if (property.PropertyType == typeof(byte))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.TinyInt;
                row[SchemaTableColumnOrdinal.DataTypeName] = "TinyInt";
            }
            else if (property.PropertyType == typeof(short))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.SmallInt;
                row[SchemaTableColumnOrdinal.DataTypeName] = "SmallInt";
            }
            else if (property.PropertyType == typeof(long))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.BigInt;
                row[SchemaTableColumnOrdinal.DataTypeName] = "BigInt";
            }
            else if (property.PropertyType == typeof(string))
            {
                row[SchemaTableColumnOrdinal.NumericPrecision] = 255;
                row[SchemaTableColumnOrdinal.NumericScale] = 255;
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.NVarChar;
                row[SchemaTableColumnOrdinal.DataTypeName] = "NVarChar";
            }
            else if (property.PropertyType == typeof(float))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.Float;
                row[SchemaTableColumnOrdinal.DataTypeName] = "Float";
            }
            else if (property.PropertyType == typeof(double))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.Real;
                row[SchemaTableColumnOrdinal.DataTypeName] = "Double";
            }
            else if (property.PropertyType == typeof(decimal))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.Decimal;
                row[SchemaTableColumnOrdinal.DataTypeName] = "Decimal";
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.DateTime2;
                row[SchemaTableColumnOrdinal.DataTypeName] = "DateTime2";
            }
            else if (property.PropertyType == typeof(Guid))
            {
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.UniqueIdentifier;
                row[SchemaTableColumnOrdinal.DataTypeName] = "UniqueIdentifier";
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                row[SchemaTableColumnOrdinal.NumericPrecision] = 255;
                row[SchemaTableColumnOrdinal.NumericScale] = 255;
                row[SchemaTableColumnOrdinal.ProviderType] = (int)SqlDbType.Binary;
                row[SchemaTableColumnOrdinal.DataTypeName] = "VarBinary";
            }
            else
            {
                throw new NotSupportedException($"The data type '{property.PropertyType.FullName}' is not supported");
            }

            dt.Rows.Add(row);
        }

        return dt;
    }

    public override bool GetBoolean(int ordinal) => (bool)this.GetValue(ordinal);

    public override byte GetByte(int ordinal) => (byte)this.GetValue(ordinal);

    public override long GetBytes(
        int ordinal,
        long dataOffset,
        byte[]? buffer,
        int bufferOffset,
        int length)
    {
        var value = (byte[])this.GetValue(ordinal);

        if (dataOffset >= value.Length)
        {
            return 0;
        }

        value.CopyTo(buffer.AsSpan());
        return value.Length;
    }

    public override char GetChar(int ordinal) => (char)this.GetValue(ordinal);

    public override long GetChars(
        int ordinal,
        long dataOffset,
        char[]? buffer,
        int bufferOffset,
        int length) => throw new NotSupportedException();

    public override string GetDataTypeName(int ordinal) => throw new NotSupportedException();

    public override DateTime GetDateTime(int ordinal) => (DateTime)this.GetValue(ordinal);

    public override decimal GetDecimal(int ordinal) => (decimal)this.GetValue(ordinal);

    public override double GetDouble(int ordinal) => (double)this.GetValue(ordinal);

    public override Type GetFieldType(int ordinal) => Properties[ordinal].PropertyType;

    public override float GetFloat(int ordinal) => (float)this.GetValue(ordinal);

    public override Guid GetGuid(int ordinal) => (Guid)this.GetValue(ordinal);

    public override short GetInt16(int ordinal) => (short)this.GetValue(ordinal);

    public override int GetInt32(int ordinal) => (int)this.GetValue(ordinal);

    public override long GetInt64(int ordinal) => (long)this.GetValue(ordinal);

    public override string GetName(int ordinal) => Properties[ordinal].Name;

    public override int GetOrdinal(string name) => throw new NotSupportedException();

    public override string GetString(int ordinal) => (string)this.GetValue(ordinal);

    public override object GetValue(int ordinal) => CompiledGetters[ordinal](this.source.Current);

    public override int GetValues(object[] values) => throw new NotSupportedException();

    public override bool IsDBNull(int ordinal) => this.GetValue(ordinal) is null;

    public override int FieldCount => Properties.Count;

    public override object this[int ordinal] => this.GetValue(ordinal);

    public override object this[string name] => throw new NotSupportedException();

    public override int RecordsAffected => throw new NotSupportedException();

    public override bool HasRows => throw new NotSupportedException();

    public override bool IsClosed => throw new NotSupportedException();

    public override bool NextResult() => throw new NotSupportedException();

    public override bool Read() => this.source.MoveNext();

    public override int Depth => -1;

    public override IEnumerator GetEnumerator() => throw new NotSupportedException();

    private class SchemaTableColumnOrdinal
    {
        public const int ColumnName = 0;
        public const int ColumnOrdinal = 1;
        public const int IsKey = 2;
        public const int DataType = 3;
        public const int ProviderType = 4;
        public const int ColumnSize = 5;
        public const int NumericPrecision = 6;
        public const int NumericScale = 7;
        public const int DataTypeName = 8;
    }
}
