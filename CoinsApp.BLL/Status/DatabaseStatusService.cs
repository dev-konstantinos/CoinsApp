using CoinsApp.DAL.Database;
using CoinsApp.DAL.Status;

namespace CoinsApp.BLL.Status;

public sealed class DatabaseStatusService
{
    private readonly DatabaseStatusProvider _databaseStatusProvider;

    public DatabaseStatusService(string connectionString)
        : this(
            new DatabaseStatusProvider(
                new DatabaseConnection(connectionString)))
    {
    }

    public DatabaseStatusService(
        DatabaseStatusProvider databaseStatusProvider)
    {
        _databaseStatusProvider =
            databaseStatusProvider
            ?? throw new ArgumentNullException(
                nameof(databaseStatusProvider));
    }

    public async Task<DatabaseStatusResult> GetStatusAsync()
    {
        return await _databaseStatusProvider.GetStatusAsync();
    }
}