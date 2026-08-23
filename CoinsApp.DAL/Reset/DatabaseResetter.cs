using CoinsApp.DAL.Database;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Reset;

public sealed class DatabaseResetter
{
    private const string ResetRelativePath = "Reset/ResetDevelopment.sql";

    private const string SeedRelativePath = "Reset/DevelopmentSeed.sql";

    private readonly DatabaseConnection _databaseConnection;

    public DatabaseResetter(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public ResetResult Reset()
    {
        try
        {
            var resetScriptPath = GetPath(ResetRelativePath);

            var seedScriptPath = GetPath(SeedRelativePath);

            if (!File.Exists(resetScriptPath))
            {
                return ResetResult.Failed($"Reset script not found: {resetScriptPath}");
            }

            if (!File.Exists(seedScriptPath))
            {
                return ResetResult.Failed($"Development seed script not found: {seedScriptPath}");
            }

            var resetScript = File.ReadAllText(resetScriptPath);

            var seedScript = File.ReadAllText(seedScriptPath);

            using var connection = _databaseConnection.Create();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                ExecuteScript(connection, transaction, resetScript);

                ExecuteScript(connection, transaction, seedScript);

                transaction.Commit();

                return ResetResult.Succeeded($"Database '{_databaseConnection.DatabaseName}' reset successfully.");
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Preserve original exception.
                }

                throw;
            }
        }
        catch (SqlException ex)
        {
            return ResetResult.Failed($"Database reset failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ResetResult.Failed($"Database reset failed: {ex.Message}");
        }
    }

    private static string GetPath(string relativePath)
    {
        return Path.Combine(AppContext.BaseDirectory, relativePath);
    }

    private static void ExecuteScript(SqlConnection connection, SqlTransaction transaction, string script)
    {
        using var command =
            new SqlCommand(script, connection, transaction)
            {
                CommandTimeout = 0
            };

        command.ExecuteNonQuery();
    }
}