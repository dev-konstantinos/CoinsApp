using System.Data;
using CoinsApp.BLL.Catalogs;
using CoinsApp.BLL.Catalogs.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.Catalogs;

public sealed class CatalogRepository : ICatalogRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CatalogRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CatalogData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var catalogs = await connection.QueryAsync<CatalogData>(
            "dbo.Catalogs_GetAll",
            commandType: CommandType.StoredProcedure);

        return catalogs.ToList();
    }

    public async Task<CatalogData?> GetByIdAsync(
        int catalogId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogData>(
            "dbo.Catalogs_GetById",
            new { CatalogId = catalogId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CatalogData?> CreateAsync(
        CatalogCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogData>(
            "dbo.Catalogs_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CatalogData?> UpdateAsync(
        CatalogUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CatalogData>(
            "dbo.Catalogs_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        CatalogDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Catalogs_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}