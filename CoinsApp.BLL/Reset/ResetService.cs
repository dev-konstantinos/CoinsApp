using CoinsApp.DAL.Database;
using CoinsApp.DAL.Reset;

namespace CoinsApp.BLL.Reset;

public sealed class ResetService
{
    private readonly DatabaseResetter _databaseResetter;

    public ResetService(string connectionString)
        : this(
            new DatabaseResetter(
                new DatabaseConnection(connectionString)))
    {
    }

    public ResetService(DatabaseResetter databaseResetter)
    {
        _databaseResetter =
            databaseResetter
            ?? throw new ArgumentNullException(nameof(databaseResetter));
    }

    public async Task<ResetResult> ResetAsync()
    {
        return await _databaseResetter.ResetAsync();
    }
}