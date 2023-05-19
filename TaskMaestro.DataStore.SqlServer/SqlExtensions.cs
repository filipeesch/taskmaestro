namespace TaskMaestro.DataStore.SqlServer;

using System.Data.SqlClient;

internal static class SqlExtensions
{
    public static DateTime? GetNullableDateTime(this SqlDataReader dr, int order)
    {
        var value = dr.GetSqlDateTime(order);

        return value.IsNull ? null : value.Value;
    }

    public static byte[]? GetNullableBinary(this SqlDataReader dr, int order)
    {
        var value = dr.GetSqlBinary(order);

        return value.IsNull ? null : value.Value;
    }
}
