using System.Data;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.Sale.Models;
using Dapper;

namespace CoinsApp.DAL.Sale;

public sealed class SaleRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public SaleRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<SaleData?> GetByIdAsync(
        int saleId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<SaleData>(
            "dbo.Sales_GetById",
            new { SaleId = saleId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<SaleData>> GetByCoinAsync(
        int coinId)
    {
        using var connection = _databaseConnection.Create();

        var sales = await connection.QueryAsync<SaleData>(
            "dbo.Sales_GetByCoin",
            new { CoinId = coinId },
            commandType: CommandType.StoredProcedure);

        return sales.ToList();
    }

    public async Task<int> CreateAsync(
        SaleCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Sales_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(
        SaleUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Sales_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        SaleDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Sales_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}