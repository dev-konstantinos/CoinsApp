using System.Data;
using CoinsApp.DAL.Features.CountryCurrencies.Models;
using CoinsApp.DAL.Technical.Database;
using Dapper;

namespace CoinsApp.DAL.Features.CountryCurrencies;

public sealed class CountryCurrencyRepository : ICountryCurrencyRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CountryCurrencyRepository(
        DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(
                nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CountryCurrencyData>>
        GetAllAsync()
    {
        using var connection =
            _databaseConnection.Create();

        var countryCurrencies =
            await connection.QueryAsync<CountryCurrencyData>(
                "dbo.CountryCurrencies_GetAll",
                commandType: CommandType.StoredProcedure);

        return countryCurrencies.ToList();
    }

    public async Task<CountryCurrencyData?>
        GetByIdAsync(int countryCurrencyId)
    {
        using var connection =
            _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<
            CountryCurrencyData>(
            "dbo.CountryCurrencies_GetById",
            new
            {
                CountryCurrencyId = countryCurrencyId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<CountryCurrencyData>>
        GetByCountryAsync(int countryId)
    {
        using var connection =
            _databaseConnection.Create();

        var countryCurrencies =
            await connection.QueryAsync<CountryCurrencyData>(
                "dbo.CountryCurrencies_GetByCountry",
                new
                {
                    CountryId = countryId
                },
                commandType: CommandType.StoredProcedure);

        return countryCurrencies.ToList();
    }

    public async Task<IReadOnlyList<CountryCurrencyData>>
        GetByCurrencyAsync(int currencyId)
    {
        using var connection =
            _databaseConnection.Create();

        var countryCurrencies =
            await connection.QueryAsync<CountryCurrencyData>(
                "dbo.CountryCurrencies_GetByCurrency",
                new
                {
                    CurrencyId = currencyId
                },
                commandType: CommandType.StoredProcedure);

        return countryCurrencies.ToList();
    }

    public async Task<CountryCurrencyData?>
        CreateAsync(CountryCurrencyCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection =
            _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<
            CountryCurrencyData>(
            "dbo.CountryCurrencies_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CountryCurrencyData?>
        SetActiveAsync(CountryCurrencySetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection =
            _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<
            CountryCurrencyData>(
            "dbo.CountryCurrencies_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}