using System.Data;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.Mints.Models;
using Dapper;

namespace CoinsApp.DAL.Mints;

public sealed class MintRepository : IMintRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public MintRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<MintData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var mints = await connection.QueryAsync<MintData>(
            "dbo.Mints_GetAll",
            commandType: CommandType.StoredProcedure);

        return mints.ToList();
    }

    public async Task<IReadOnlyList<MintData>> GetByCountryAsync(
        int countryId)
    {
        using var connection = _databaseConnection.Create();

        var mints = await connection.QueryAsync<MintData>(
            "dbo.Mints_GetByCountry",
            new
            {
                CountryId = countryId
            },
            commandType: CommandType.StoredProcedure);

        return mints.ToList();
    }

    public async Task<MintData?> GetByIdAsync(int mintId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MintData>(
            "dbo.Mints_GetById",
            new
            {
                MintId = mintId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MintData?> CreateAsync(MintCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MintData>(
            "dbo.Mints_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MintData?> UpdateAsync(MintUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MintData>(
            "dbo.Mints_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MintData?> SetActiveAsync(
        MintSetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MintData>(
            "dbo.Mints_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}