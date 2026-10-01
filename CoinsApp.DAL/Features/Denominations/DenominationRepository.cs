using System.Data;
using CoinsApp.DAL.Features.Denominations.Models;
using CoinsApp.DAL.Technical.Database;
using Dapper;

namespace CoinsApp.DAL.Features.Denominations;

public sealed class DenominationRepository : IDenominationRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public DenominationRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<DenominationData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var denominations = await connection.QueryAsync<DenominationData>(
            "dbo.Denominations_GetAll",
            commandType: CommandType.StoredProcedure);

        return denominations.ToList();
    }

    public async Task<IReadOnlyList<DenominationData>> GetByCurrencyAsync(
        int currencyId)
    {
        using var connection = _databaseConnection.Create();

        var denominations = await connection.QueryAsync<DenominationData>(
            "dbo.Denominations_GetByCurrency",
            new
            {
                CurrencyId = currencyId
            },
            commandType: CommandType.StoredProcedure);

        return denominations.ToList();
    }

    public async Task<DenominationData?> GetByIdAsync(
        int denominationId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<DenominationData>(
            "dbo.Denominations_GetById",
            new
            {
                DenominationId = denominationId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<DenominationData?> CreateAsync(
        DenominationCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<DenominationData>(
            "dbo.Denominations_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<DenominationData?> UpdateAsync(
        DenominationUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<DenominationData>(
            "dbo.Denominations_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<DenominationData?> SetActiveAsync(
        DenominationSetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<DenominationData>(
            "dbo.Denominations_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}