namespace CoinsApp.BLL.Install;

public sealed class InstallService : IInstallService
{
    private readonly IDatabaseInstaller _databaseInstaller;

    public InstallService(IDatabaseInstaller databaseInstaller)
    {
        _databaseInstaller = databaseInstaller ?? throw new ArgumentNullException(nameof(databaseInstaller));
    }

    public InstallResult Install()
    {
        try
        {
            _databaseInstaller.Install();

            return InstallResult.Succeeded("Installation completed successfully.");
        }
        catch (Exception ex)
        {
            return InstallResult.Failed($"Installation failed: {ex.Message}");
        }
    }
}