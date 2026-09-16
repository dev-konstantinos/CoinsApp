using System.Data;
using CoinsApp.DAL.CatalogEntries.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.CatalogEntries;

public sealed class CatalogEntryRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CatalogEntryRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CatalogEntryData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var entries = await connection.QueryAsync<CatalogEntryData>(
            "dbo.CatalogEntries_GetAll",
            commandType: CommandType.StoredProcedure);

        return entries.ToList();
    }

    public async Task<CatalogEntryData?> GetByIdAsync(
        int catalogEntryId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogEntryData>(
            "dbo.CatalogEntries_GetById",
            new
            {
                CatalogEntryId = catalogEntryId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<CatalogEntryData>> GetByCoinAsync(
        int coinId)
    {
        using var connection = _databaseConnection.Create();

        var entries = await connection.QueryAsync<CatalogEntryData>(
            "dbo.CatalogEntries_GetByCoin",
            new
            {
                CoinId = coinId
            },
            commandType: CommandType.StoredProcedure);

        return entries.ToList();
    }

    public async Task<IReadOnlyList<CatalogEntryData>> GetByCatalogAsync(
        int catalogId)
    {
        using var connection = _databaseConnection.Create();

        var entries = await connection.QueryAsync<CatalogEntryData>(
            "dbo.CatalogEntries_GetByCatalog",
            new
            {
                CatalogId = catalogId
            },
            commandType: CommandType.StoredProcedure);

        return entries.ToList();
    }

    public async Task<CatalogEntryData?> CreateAsync(
        CatalogEntryCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogEntryData>(
            "dbo.CatalogEntries_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CatalogEntryData?> UpdateAsync(
        CatalogEntryUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogEntryData>(
            "dbo.CatalogEntries_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        CatalogEntryDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.CatalogEntries_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}