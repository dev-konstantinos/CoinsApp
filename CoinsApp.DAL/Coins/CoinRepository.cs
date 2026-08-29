using CoinsApp.DAL.Coins.Models;
using CoinsApp.DAL.Database;
using Dapper;
using System.Data;

namespace CoinsApp.DAL.Coins;

public sealed class CoinRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CoinRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public IReadOnlyList<CoinData> GetAll()
    {
        using var connection = _databaseConnection.Create();

        var coins = connection.Query<CoinData>("dbo.Coins_GetAll", commandType: CommandType.StoredProcedure);

        return coins.ToList();
    }

    public CoinData? GetById(int coinId)
    {
        using var connection = _databaseConnection.Create();

        return connection.QuerySingleOrDefault<CoinData>("dbo.Coins_GetById", new { CoinId = coinId },
            commandType: CommandType.StoredProcedure);
    }

    public int Create(CoinCreateData data)
    {
        using var connection = _databaseConnection.Create();

        return connection.QuerySingle<int>("dbo.Coins_Create", data, commandType: CommandType.StoredProcedure);
    }
}