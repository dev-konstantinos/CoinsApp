using System.Data;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.Users.Models;
using Dapper;

namespace CoinsApp.DAL.Users;

public sealed class UserRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public UserRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<UserData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var users = await connection.QueryAsync<UserData>(
            "dbo.Users_GetAll",
            commandType: CommandType.StoredProcedure);

        return users.ToList();
    }

    public async Task<UserData?> GetByIdAsync(int userId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<UserData>(
            "dbo.Users_GetById",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(UserCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Users_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(UserUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Users_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> SetPasswordAsync(UserSetPasswordData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Users_SetPassword",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(UserDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Users_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}