namespace CoinsApp.BLL.Install;

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