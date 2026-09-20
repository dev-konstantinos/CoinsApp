using System.Data;
using CoinsApp.DAL.Countries.Models;
using CoinsApp.DAL.Database;
using Dapper;

namespace CoinsApp.DAL.Countries;

public sealed class CountryRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public CountryRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<CountryData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var countries = await connection.QueryAsync<CountryData>(
            "dbo.Countries_GetAll",
            commandType: CommandType.StoredProcedure);

        return countries.ToList();
    }

    public async Task<CountryData?> GetByIdAsync(int countryId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CountryData>(
            "dbo.Countries_GetById",
            new
            {
                CountryId = countryId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CountryData?> CreateAsync(CountryCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CountryData>(
            "dbo.Countries_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CountryData?> UpdateAsync(CountryUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CountryData>(
            "dbo.Countries_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CountryData?> SetActiveAsync(
        CountrySetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<CountryData>(
            "dbo.Countries_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}