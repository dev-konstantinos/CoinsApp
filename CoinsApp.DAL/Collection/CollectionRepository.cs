using System.Data;
using CoinsApp.DAL.Collection.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.Collection;

public sealed class CollectionRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CollectionRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CollectionData>> GetAllAsync(
        int? userId = null)
    {
        using var connection = _databaseConnection.Create();

        var collections = await connection.QueryAsync<CollectionData>(
            "dbo.Collections_GetAll",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);

        return collections.ToList();
    }

    public async Task<CollectionData?> GetByIdAsync(int collectionId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CollectionData>(
            "dbo.Collections_GetById",
            new { CollectionId = collectionId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CollectionCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Collections_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CollectionData?> UpdateAsync(
        CollectionUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CollectionData>(
            "dbo.Collections_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(CollectionDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Collections_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}