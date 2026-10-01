namespace CoinsApp.DAL.Features.Users.Models;

public sealed class UserUpdateData
{
    public int UserId { get; init; }

    public string Username { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; }
}