using System.Data;
using CoinsApp.DAL.Features.PriceHistories.Models;
using CoinsApp.DAL.Technical.Database;
using Dapper;

namespace CoinsApp.DAL.Features.PriceHistories;

public sealed class PriceHistoryRepository : IPriceHistoryRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public PriceHistoryRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<PriceHistoryData>> GetByCoinAsync(
        int coinId)
    {
        using var connection = _databaseConnection.Create();

        var entries = await connection.QueryAsync<PriceHistoryData>(
            "dbo.PriceHistory_GetByCoin",
            new { CoinId = coinId },
            commandType: CommandType.StoredProcedure);

        return entries.ToList();
    }

    public async Task<int> CreateAsync(
        PriceHistoryCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.PriceHistory_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(
        PriceHistoryUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.PriceHistory_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        PriceHistoryDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.PriceHistory_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}