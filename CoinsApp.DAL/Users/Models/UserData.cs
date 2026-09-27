namespace CoinsApp.DAL.Users.Models;

public sealed class UserData
{
    public int UserId { get; init; }

    public string Username { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }

    public int CollectionCount { get; init; }
}