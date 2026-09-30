using CoinsApp.DAL.Database;
using CoinsApp.DAL.Purchase.Models;
using Dapper;
using System.Data;

namespace CoinsApp.DAL.Purchase;

public sealed class PurchaseRepository : IPurchaseRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public PurchaseRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<PurchaseData?> GetByIdAsync(
        int purchaseId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<PurchaseData>(
            "dbo.Purchases_GetById",
            new { PurchaseId = purchaseId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<PurchaseData>> GetByCoinAsync(
        int coinId)
    {
        using var connection = _databaseConnection.Create();

        var purchases = await connection.QueryAsync<PurchaseData>(
            "dbo.Purchases_GetByCoin",
            new { CoinId = coinId },
            commandType: CommandType.StoredProcedure);

        return purchases.ToList();
    }

    public async Task<int> CreateAsync(
        PurchaseCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Purchases_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(
        PurchaseUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Purchases_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        PurchaseDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Purchases_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}