using CoinsApp.DAL.Database;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Reset;

public sealed class DatabaseResetter
{
    private readonly DatabaseConnection _databaseConnection;

    public DatabaseResetter(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public ResetResult Reset()
    {
        var resetScriptPath = Path.Combine(AppContext.BaseDirectory, "Reset", "ResetDevelopment.sql");

        var seedScriptPath = Path.Combine(AppContext.BaseDirectory, "Reset", "DevelopmentSeed.sql");

        if (!File.Exists(resetScriptPath))
        {
            return ResetResult.Failed($"Reset script not found: {resetScriptPath}");
        }

        if (!File.Exists(seedScriptPath))
        {
            return ResetResult.Failed($"Development seed script not found: {seedScriptPath}");
        }

        try
        {
            var resetScript = File.ReadAllText(resetScriptPath);
            var seedScript = File.ReadAllText(seedScriptPath);

            using var connection = new SqlConnection(_databaseConnection.ConnectionString);

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                ExecuteScript(connection, transaction, resetScript);

                ExecuteScript(connection, transaction, seedScript);

                transaction.Commit();

                return ResetResult.Success();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Preserve the original reset exception.
                }

                throw;
            }
        }
        catch (Exception ex)
        {
            return ResetResult.Failed(ex.Message);
        }
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