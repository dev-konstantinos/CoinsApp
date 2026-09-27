namespace CoinsApp.DAL.Users.Models;

public sealed class UserSetPasswordData
{
    public int UserId { get; init; }

    public string PasswordHash { get; init; } = string.Empty;
}