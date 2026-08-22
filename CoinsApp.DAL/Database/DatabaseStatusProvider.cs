using CoinsApp.BLL.Database;
using CoinsApp.DAL.Configuration;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Database;

public sealed class DatabaseStatusProvider : IDatabaseStatusService
{
    private const string InstallationCheckSql = """
        SELECT CASE
            WHEN OBJECT_ID(N'dbo.Coins', N'U') IS NOT NULL
             AND OBJECT_ID(N'dbo.Currencies', N'U') IS NOT NULL
             AND OBJECT_ID(N'dbo.Countries', N'U') IS NOT NULL
            THEN CAST(1 AS bit)
            ELSE CAST(0 AS bit)
        END;
        """;

    private readonly DatabaseOptions _options;

    public DatabaseStatusProvider(DatabaseOptions options)
    {
        _options = options
            ?? throw new ArgumentNullException(nameof(options));
    }

    public DatabaseStatusResult GetStatus()
    {
        try
        {
            using var connection = new SqlConnection(
                _options.ConnectionString);

            connection.Open();

            var isInstalled = CheckInstallation(connection);

            if (isInstalled)
            {
                return DatabaseStatusResult.Connected(
                    isInstalled: true,
                    message: "Database is connected and CoinsApp schema is installed.");
            }

            return DatabaseStatusResult.Connected(
                isInstalled: false,
                message: "Database is connected, but CoinsApp schema is not installed.");
        }
        catch (SqlException ex)
        {
            return DatabaseStatusResult.Disconnected(
                $"Database connection failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return DatabaseStatusResult.Disconnected(
                $"Database status check failed: {ex.Message}");
        }
    }

    private static bool CheckInstallation(SqlConnection connection)
    {
        using var command = new SqlCommand(
            InstallationCheckSql,
            connection);

        var result = command.ExecuteScalar();

        return result is bool isInstalled && isInstalled;
    }
}