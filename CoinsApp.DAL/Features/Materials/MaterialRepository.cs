using System.Data;
using CoinsApp.DAL.Features.Materials.Models;
using CoinsApp.DAL.Technical.Database;
using Dapper;

namespace CoinsApp.DAL.Features.Materials;

public sealed class MaterialRepository : IMaterialRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public MaterialRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<MaterialData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var materials = await connection.QueryAsync<MaterialData>(
            "dbo.Materials_GetAll",
            commandType: CommandType.StoredProcedure);

        return materials.ToList();
    }

    public async Task<MaterialData?> GetByIdAsync(int materialId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MaterialData>(
            "dbo.Materials_GetById",
            new
            {
                MaterialId = materialId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MaterialData?> CreateAsync(MaterialCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MaterialData>(
            "dbo.Materials_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MaterialData?> UpdateAsync(MaterialUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MaterialData>(
            "dbo.Materials_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MaterialData?> SetActiveAsync(MaterialSetActiveData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<MaterialData>(
            "dbo.Materials_SetActive",
            data,
            commandType: CommandType.StoredProcedure);
    }
}
