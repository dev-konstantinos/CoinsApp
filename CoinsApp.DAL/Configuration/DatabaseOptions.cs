namespace CoinsApp.DAL.Configuration;

public sealed class DatabaseOptions
{
    public string ConnectionString { get; }

    public DatabaseOptions(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Database connection string cannot be empty!");

        ConnectionString = connectionString;
    }
}