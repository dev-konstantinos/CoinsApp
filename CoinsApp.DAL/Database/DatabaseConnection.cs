using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Database;

public sealed class DatabaseConnection
{
    private readonly string _connectionString;

    public string ConnectionString => _connectionString;

    public string DatabaseName { get; }

    public DatabaseConnection(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string cannot be empty.");
        }

        var builder = new SqlConnectionStringBuilder(connectionString);

        if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
        {
            throw new ArgumentException("Database name is missing from the connection string.");
        }

        _connectionString = connectionString;
        DatabaseName = builder.InitialCatalog;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}