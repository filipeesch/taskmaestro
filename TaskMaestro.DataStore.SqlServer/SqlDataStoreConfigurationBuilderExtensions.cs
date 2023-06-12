namespace TaskMaestro.DataStore.SqlServer;

using System.Data.SqlClient;
using TaskMaestro.Setup;

public static class SqlDataStoreConfigurationBuilderExtensions
{
    public static MaestroConfigurationBuilder UseSqlDataStore(
        this MaestroConfigurationBuilder builder,
        Func<IServiceProvider, SqlConnection> factory)
    {
        return builder.UseDataStore(provider => new SqlServerDataStore(() => factory(provider)));
    }

    public static MaestroConfigurationBuilder UseSqlDataStore(this MaestroConfigurationBuilder builder, string connectionString)
    {
        return builder.UseSqlDataStore(_ => new SqlConnection(connectionString));
    }
}
