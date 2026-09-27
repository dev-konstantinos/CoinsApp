namespace CoinsApp.BLL.Users.ViewModels;

public sealed class UpdateUserViewModel
{
    public int UserId { get; init; }

    public string Username { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; }
}