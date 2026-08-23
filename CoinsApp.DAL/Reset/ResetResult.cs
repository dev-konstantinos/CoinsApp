namespace CoinsApp.DAL.Reset;

public sealed class ResetResult
{
    public bool IsSuccess { get; }

    public string? ErrorMessage { get; }

    private ResetResult(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ResetResult Success()
    {
        return new ResetResult(isSuccess: true, errorMessage: null);
    }

    public static ResetResult Failed(string errorMessage)
    {
        return new ResetResult(isSuccess: false, errorMessage: errorMessage);
    }
}