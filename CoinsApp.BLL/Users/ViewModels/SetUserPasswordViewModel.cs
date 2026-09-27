namespace CoinsApp.BLL.Users.ViewModels;

public sealed class SetUserPasswordViewModel
{
    public int UserId { get; init; }

    public string NewPassword { get; init; } = string.Empty;

    public string ConfirmPassword { get; init; } = string.Empty;
}