namespace CoinsApp.BLL.Contacts.ViewModels;

public sealed class UpdateContactViewModel
{
    public int ContactId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? CompanyName { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public string? Address { get; init; }

    public string? Website { get; init; }

    public string? Notes { get; init; }

    public bool IsActive { get; init; }
}