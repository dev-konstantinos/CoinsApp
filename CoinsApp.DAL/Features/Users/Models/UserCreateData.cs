namespace CoinsApp.DAL.Features.Users.Models;

public sealed class UserCreateData
{
    public string Username { get; init; } = string.Empty;

    public string PasswordHash { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; } = true;
}