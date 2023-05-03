namespace TaskMaestro.DataStore.SqlServer;

internal static class EnumerableExtensions
{
    public static EnumerableDataReader<T> ToDataReader<T>(this IEnumerable<T> source) => new EnumerableDataReader<T>(source);
}