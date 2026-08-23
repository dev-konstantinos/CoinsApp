using CoinsApp.DAL.Database;
using CoinsApp.DAL.Install;

namespace CoinsApp.BLL.Install;

public sealed class InstallService
{
    private readonly DatabaseInstaller _databaseInstaller;

    public InstallService(string connectionString)
        : this(new DatabaseInstaller(new DatabaseConnection(connectionString)))
    {
    }

    public InstallService(DatabaseInstaller databaseInstaller)
    {
        _databaseInstaller = databaseInstaller ?? throw new ArgumentNullException(nameof(databaseInstaller));
    }

    public InstallResult Install()
    {
        return _databaseInstaller.Install();
    }
}