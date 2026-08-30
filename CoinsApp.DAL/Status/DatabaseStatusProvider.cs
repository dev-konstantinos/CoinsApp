using CoinsApp.DAL.Database;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Status;

public sealed class DatabaseStatusProvider
{
    private readonly DatabaseConnection _databaseConnection;

    public DatabaseStatusProvider(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<DatabaseStatusResult> GetStatusAsync()
    {
        try
        {
            using var connection = _databaseConnection.Create();

            await connection.OpenAsync();

            const string sql = """
                SELECT
                    CAST(SERVERPROPERTY('ServerName') AS nvarchar(128)) AS ServerName,
                    DB_NAME() AS DatabaseName,
                    CAST(SERVERPROPERTY('ProductVersion') AS nvarchar(128)) AS ProductVersion,
                    CAST(SERVERPROPERTY('Edition') AS nvarchar(128)) AS Edition,
                    CAST(SERVERPROPERTY('ProductLevel') AS nvarchar(128)) AS ProductLevel;
                """;

            using var command = new SqlCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return DatabaseStatusResult.Failed("Database status information could not be read.");
            }

            var serverName =
                reader["ServerName"]?.ToString()
                ?? "Unknown";

            var databaseName = reader["DatabaseName"]?.ToString() ?? "Unknown";

            var sqlServerVersion = reader["ProductVersion"]?.ToString() ?? "Unknown";

            var sqlServerEdition = reader["Edition"]?.ToString() ?? "Unknown";

            var sqlServerLevel = reader["ProductLevel"]?.ToString() ?? "Unknown";

            return DatabaseStatusResult.Succeeded(
                serverName,
                databaseName,
                sqlServerVersion,
                sqlServerEdition,
                sqlServerLevel);
        }
        catch (SqlException ex)
        {
            return DatabaseStatusResult.Failed($"Database is not available. {ex.Message}");
        }
        catch (Exception ex)
        {
            return DatabaseStatusResult.Failed($"Database status check failed. {ex.Message}");
        }
    }
}