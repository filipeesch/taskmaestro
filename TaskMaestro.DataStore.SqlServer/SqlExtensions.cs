namespace TaskMaestro.DataStore.SqlServer;

using System.Data.SqlClient;

internal static class SqlExtensions
{
    public static T? GetNullable<T>(this SqlDataReader dr, int order)
    {
        var value = dr.GetValue(order);

        return value == DBNull.Value ? default : (T?)value;
    }
}
