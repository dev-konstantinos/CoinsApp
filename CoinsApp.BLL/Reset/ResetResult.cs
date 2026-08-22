namespace CoinsApp.BLL.Reset;

public sealed class ResetResult
{
    public bool Success { get; }

    public string Message { get; }

    private ResetResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static ResetResult Succeeded(string message)
    {
        return new ResetResult(true, message);
    }

    public static ResetResult Failed(string message)
    {
        return new ResetResult(false, message);
    }
}