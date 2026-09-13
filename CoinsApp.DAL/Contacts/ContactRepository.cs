using CoinsApp.DAL.Contacts.Models;
using CoinsApp.DAL.Database;
using Dapper;
using System.Data;

namespace CoinsApp.DAL.Contacts;

public sealed class ContactRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public ContactRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection =
            databaseConnection
            ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IReadOnlyList<ContactData>> GetAllAsync()
    {
        using var connection = _databaseConnection.Create();

        var contacts = await connection.QueryAsync<ContactData>(
            "dbo.Contacts_GetAll",
            commandType: CommandType.StoredProcedure);

        return contacts.ToList();
    }

    public async Task<ContactData?> GetByIdAsync(
        int contactId)
    {
        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<ContactData>(
            "dbo.Contacts_GetById",
            new { ContactId = contactId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(
        ContactCreateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Contacts_Create",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ContactData?> UpdateAsync(
        ContactUpdateData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleOrDefaultAsync<ContactData>(
            "dbo.Contacts_Update",
            data,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> DeleteAsync(
        ContactDeleteData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var connection = _databaseConnection.Create();

        return await connection.QuerySingleAsync<int>(
            "dbo.Contacts_Delete",
            data,
            commandType: CommandType.StoredProcedure);
    }
}