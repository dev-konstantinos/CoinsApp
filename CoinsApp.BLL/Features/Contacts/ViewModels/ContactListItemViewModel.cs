namespace CoinsApp.BLL.Features.Contacts.ViewModels;

public sealed class ContactListItemViewModel
{
    public int ContactId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? CompanyName { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public bool IsActive { get; init; }
}