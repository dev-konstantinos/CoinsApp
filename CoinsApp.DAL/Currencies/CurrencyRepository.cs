using System.Data;
using CoinsApp.DAL.Currencies.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.Currencies;

public sealed class CurrencyRepository : ICurrencyRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CurrencyRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CurrencyData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var currencies = await connection.QueryAsync<CurrencyData>(
            "dbo.Currencies_GetAll",
            commandType: CommandType.StoredProcedure);

        return currencies.ToList();
    }

    public async Task<CurrencyData?> GetByIdAsync(int currencyId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CurrencyData>(
            "dbo.Currencies_GetById",
            new
            {
                CurrencyId = currencyId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CurrencyData?> CreateAsync(
        CurrencyCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CurrencyData>(
            "dbo.Currencies_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CurrencyData?> UpdateAsync(
        CurrencyUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CurrencyData>(
            "dbo.Currencies_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CurrencyData?> SetActiveAsync(
        CurrencySetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CurrencyData>(
            "dbo.Currencies_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}