namespace CoinsApp.BLL.Features.Contacts.ViewModels;

public sealed class CreateContactViewModel
{
    public string Name { get; init; } = string.Empty;

    public string? CompanyName { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public string? Address { get; init; }

    public string? Website { get; init; }

    public string? Notes { get; init; }

    public bool IsActive { get; init; } = true;
}