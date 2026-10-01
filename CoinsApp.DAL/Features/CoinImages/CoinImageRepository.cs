using System.Data;
using CoinsApp.DAL.Features.CoinImages.Models;
using CoinsApp.DAL.Technical.Database;
using Dapper;

namespace CoinsApp.DAL.Features.CoinImages;

public sealed class CoinImageRepository : ICoinImageRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CoinImageRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CoinImageData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var images = await connection.QueryAsync<CoinImageData>(
            "dbo.CoinImages_GetAll",
            commandType: CommandType.StoredProcedure);

        return images.ToList();
    }

    public async Task<CoinImageData?> GetByIdAsync(
        int coinImageId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CoinImageData>(
            "dbo.CoinImages_GetById",
            new
            {
                CoinImageId = coinImageId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<CoinImageData>> GetByCoinAsync(
        int coinId)
    {
        using var connection = _databaseConnection.Create();

        var images = await connection.QueryAsync<CoinImageData>(
            "dbo.CoinImages_GetByCoin",
            new
            {
                CoinId = coinId
            },
            commandType: CommandType.StoredProcedure);

        return images.ToList();
    }

    public async Task<CoinImageData?> CreateAsync(
        CoinImageCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CoinImageData>(
            "dbo.CoinImages_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CoinImageData?> UpdateAsync(
        CoinImageUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CoinImageData>(
            "dbo.CoinImages_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        CoinImageDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.CoinImages_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}