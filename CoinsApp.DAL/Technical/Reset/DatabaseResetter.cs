using CoinsApp.DAL.Technical.Database;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Technical.Reset;

public sealed class DatabaseResetter
{
    private const string ResetRelativePath =
        "Reset/ResetDevelopment.sql";

    private const string SeedRelativePath =
        "Reset/DevelopmentSeed.sql";

    private readonly DatabaseConnection _databaseConnection;

    public DatabaseResetter(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<ResetResult> ResetAsync()
    {
        try
        {
            var resetScriptPath = GetPath(ResetRelativePath);
            var seedScriptPath = GetPath(SeedRelativePath);

            if (!File.Exists(resetScriptPath))
            {
                return ResetResult.Failed(
                    $"Reset script not found: {resetScriptPath}");
            }

            if (!File.Exists(seedScriptPath))
            {
                return ResetResult.Failed(
                    $"Development seed script not found: {seedScriptPath}");
            }

            var resetScript =
                await File.ReadAllTextAsync(resetScriptPath);

            var seedScript =
                await File.ReadAllTextAsync(seedScriptPath);

            using var connection = _databaseConnection.Create();

            await connection.OpenAsync();

            using var transaction =
                (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                await ExecuteScriptAsync(
                    connection,
                    transaction,
                    resetScript);

                await ExecuteScriptAsync(
                    connection,
                    transaction,
                    seedScript);

                await transaction.CommitAsync();

                return ResetResult.Succeeded(
                    $"Database '{_databaseConnection.DatabaseName}' reset successfully.");
            }
            catch
            {
                try
                {
                    await transaction.RollbackAsync();
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
            return ResetResult.Failed(
                $"Database reset failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ResetResult.Failed(
                $"Database reset failed: {ex.Message}");
        }
    }

    private static string GetPath(string relativePath)
    {
        return Path.Combine(
            AppContext.BaseDirectory,
            relativePath);
    }

    private static async Task ExecuteScriptAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string script)
    {
        using var command =
            new SqlCommand(script, connection, transaction)
            {
                CommandTimeout = 0
            };

        await command.ExecuteNonQueryAsync();
    }
}