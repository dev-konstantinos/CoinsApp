using CoinsApp.DAL.Collection.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CoinsApp.DAL.Collection;

public sealed class CollectionRepository
{
    private readonly string _connectionString;

    public CollectionRepository(string connectionString)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<int> CreateAsync(
        CollectionCreateData data,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            "dbo.Collections_Create",
            new
            {
                data.UserId,
                data.Name,
                data.Description,
                data.IsActive
            },
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<int>(command);
    }

    public async Task<IReadOnlyList<CollectionData>> GetAllAsync(
        int? userId = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            "dbo.Collections_GetAll",
            new
            {
                UserId = userId
            },
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var result = await connection.QueryAsync<CollectionData>(command);

        return result.AsList();
    }

    public async Task<CollectionData?> GetByIdAsync(
        int collectionId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            "dbo.Collections_GetById",
            new
            {
                CollectionId = collectionId
            },
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<CollectionData>(command);
    }

    public async Task<CollectionData?> UpdateAsync(
        CollectionUpdateData data,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            "dbo.Collections_Update",
            new
            {
                data.CollectionId,
                data.Name,
                data.Description,
                data.IsActive
            },
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<CollectionData>(command);
    }

    public async Task<int> DeleteAsync(
        CollectionDeleteData data,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            "dbo.Collections_Delete",
            new
            {
                data.CollectionId
            },
            commandType: System.Data.CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<int>(command);
    }
}