using CoinsApp.DAL.Install;

namespace CoinsApp.BLL.Install;

public sealed class InstallService
{
    private readonly DatabaseInstaller _databaseInstaller;

    public InstallService(DatabaseInstaller databaseInstaller)
    {
        _databaseInstaller = databaseInstaller ?? throw new ArgumentNullException(nameof(databaseInstaller));
    }

    public InstallResult Install()
    {
        var result = _databaseInstaller.Install();

        if (result.Success)
        {
            return InstallResult.Succeeded(
                result.Message);
        }

        return InstallResult.Failed(
            result.Message);
    }
}

public sealed class InstallResult
{
    public bool Success { get; }

    public string Message { get; }

    private InstallResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static InstallResult Succeeded(string message)
    {
        return new InstallResult(true, message);
    }

    public static InstallResult Failed(string message)
    {
        return new InstallResult(false, message);
    }
}