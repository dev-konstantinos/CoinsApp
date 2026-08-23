using CoinsApp.DAL.Database;
using Microsoft.SqlServer.Dac;

namespace CoinsApp.DAL.Install;

public sealed class DatabaseInstaller
{
    private const string DacpacRelativePath = "Database/CoinsApp.DB.dacpac";

    private readonly DatabaseConnection _databaseConnection;

    public DatabaseInstaller(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public InstallResult Install()
    {
        try
        {
            var dacpacPath = GetDacpacPath();

            if (!File.Exists(dacpacPath))
                return InstallResult.Failed($"DACPAC not found: {dacpacPath}");

            var connectionString = _databaseConnection.ConnectionString;

            var dacServices = new DacServices(connectionString);

            using var dacpac = DacPackage.Load(dacpacPath);

            dacServices.Deploy(dacpac, "CoinsApp", upgradeExisting: true);

            return InstallResult.Succeeded("CoinsApp database installed successfully.");
        }
        catch (DacServicesException ex)
        {
            return InstallResult.Failed($"Database deployment failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return InstallResult.Failed($"Database installation failed: {ex.Message}");
        }
    }

    private static string GetDacpacPath()
    {
        return Path.Combine(AppContext.BaseDirectory, DacpacRelativePath);
    }
}