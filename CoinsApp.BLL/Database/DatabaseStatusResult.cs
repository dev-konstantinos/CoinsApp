namespace CoinsApp.BLL.Database;

public sealed class DatabaseStatusResult
{
    public bool IsConnected { get; }

    public bool IsInstalled { get; }

    public string Message { get; }

    private DatabaseStatusResult(
        bool isConnected,
        bool isInstalled,
        string message)
    {
        IsConnected = isConnected;
        IsInstalled = isInstalled;
        Message = message;
    }

    public static DatabaseStatusResult Connected(
        bool isInstalled,
        string message)
    {
        return new DatabaseStatusResult(
            true,
            isInstalled,
            message);
    }

    public static DatabaseStatusResult Disconnected(
        string message)
    {
        return new DatabaseStatusResult(
            false,
            false,
            message);
    }
}