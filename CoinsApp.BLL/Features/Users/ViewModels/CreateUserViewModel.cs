namespace CoinsApp.BLL.Features.Users.ViewModels;

public sealed class CreateUserViewModel
{
    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string ConfirmPassword { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; } = true;
}