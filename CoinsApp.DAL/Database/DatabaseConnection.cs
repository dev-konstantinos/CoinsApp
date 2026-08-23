using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Database;

public sealed class DatabaseConnection
{
    private readonly string _connectionString;

    public string ConnectionString => _connectionString;

    public DatabaseConnection(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string cannot be empty.");
        }

        _connectionString = connectionString;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}