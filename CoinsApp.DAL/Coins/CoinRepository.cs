using System.Data;
using CoinsApp.DAL.Coins.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.Coins;

public sealed class CoinRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CoinRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CoinData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var coins = await connection.QueryAsync<CoinData>(
            "dbo.Coins_GetAll",
            commandType: CommandType.StoredProcedure);

        return coins.ToList();
    }

    public async Task<CoinData?> GetByIdAsync(int coinId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CoinData>(
            "dbo.Coins_GetById",
            new { CoinId = coinId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CoinCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Coins_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(CoinUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Coins_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(CoinDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Coins_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}