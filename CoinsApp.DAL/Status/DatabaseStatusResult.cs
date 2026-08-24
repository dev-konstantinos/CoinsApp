namespace CoinsApp.DAL.Status;

public sealed class DatabaseStatusResult
{
    public bool Success { get; }

    public string Message { get; }

    public string? ServerName { get; }

    public string? DatabaseName { get; }

    public string? SqlServerVersion { get; }

    public string? SqlServerEdition { get; }

    public string? SqlServerLevel { get; }

    public DatabaseStatusResult(
        bool success,
        string message,
        string? serverName = null,
        string? databaseName = null,
        string? sqlServerVersion = null,
        string? sqlServerEdition = null,
        string? sqlServerLevel = null)
    {
        Success = success;
        Message = message;
        ServerName = serverName;
        DatabaseName = databaseName;
        SqlServerVersion = sqlServerVersion;
        SqlServerEdition = sqlServerEdition;
        SqlServerLevel = sqlServerLevel;
    }

    public static DatabaseStatusResult Succeeded(
        string serverName,
        string databaseName,
        string sqlServerVersion,
        string sqlServerEdition,
        string sqlServerLevel)
    {
        return new DatabaseStatusResult(
            success: true,
            message: "Database is available.",
            serverName: serverName,
            databaseName: databaseName,
            sqlServerVersion: sqlServerVersion,
            sqlServerEdition: sqlServerEdition,
            sqlServerLevel: sqlServerLevel);
    }

    public static DatabaseStatusResult Failed(string message)
    {
        return new DatabaseStatusResult(success: false,  message: message);
    }
}