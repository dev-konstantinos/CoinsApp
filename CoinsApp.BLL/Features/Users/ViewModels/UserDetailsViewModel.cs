namespace CoinsApp.BLL.Features.Users.ViewModels;

public sealed class UserDetailsViewModel
{
    public int UserId { get; init; }

    public string Username { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }

    public int CollectionCount { get; init; }
}