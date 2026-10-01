using CoinsApp.DAL.Technical.Database;
using CoinsApp.DAL.Technical.Install;

namespace CoinsApp.BLL.Technical.Install;

public sealed class InstallService
{
    private readonly DatabaseInstaller _databaseInstaller;

    public InstallService(string connectionString) : this(new DatabaseInstaller(new DatabaseConnection(connectionString)))
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