using CoinsApp.BLL.Contacts.ViewModels;
using CoinsApp.DAL.Contacts;
using CoinsApp.DAL.Contacts.Models;

namespace CoinsApp.BLL.Contacts;

public sealed class ContactService
{
    private readonly ContactRepository _repository;

    public ContactService(ContactRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<ContactListItemViewModel>> GetAllAsync()
    {
        var contacts =
            await _repository.GetAllAsync();

        return contacts
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<ContactDetailsViewModel?> GetByIdAsync(
        int contactId)
    {
        if (contactId <= 0)
        {
            throw new ArgumentException(
                "Contact ID must be greater than zero.",
                nameof(contactId));
        }

        var contact =
            await _repository.GetByIdAsync(contactId);

        return contact is null
            ? null
            : MapToDetails(contact);
    }

    public async Task<int> CreateAsync(
        CreateContactViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new ContactCreateData
        {
            Name = model.Name.Trim(),
            CompanyName = model.CompanyName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            Website = model.Website,
            Notes = model.Notes,
            IsActive = model.IsActive
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<int> UpdateAsync(
        UpdateContactViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new ContactUpdateData
        {
            ContactId = model.ContactId,
            Name = model.Name.Trim(),
            CompanyName = model.CompanyName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            Website = model.Website,
            Notes = model.Notes,
            IsActive = model.IsActive
        };

        return await _repository.UpdateAsync(data);
    }

    public async Task<int> DeleteAsync(
        DeleteContactViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.ContactId <= 0)
        {
            throw new ArgumentException(
                "Contact ID must be greater than zero.",
                nameof(model.ContactId));
        }

        var data = new ContactDeleteData
        {
            ContactId = model.ContactId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateContactViewModel model)
    {
        ValidateName(model.Name);
    }

    private static void ValidateUpdate(
        UpdateContactViewModel model)
    {
        if (model.ContactId <= 0)
        {
            throw new ArgumentException(
                "Contact ID must be greater than zero.",
                nameof(model.ContactId));
        }

        ValidateName(model.Name);
    }

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Contact name is required.",
                nameof(name));
        }

        if (name.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Contact name cannot exceed 150 characters.",
                nameof(name));
        }
    }

    private static ContactListItemViewModel MapToListItem(
        ContactData contact)
    {
        return new ContactListItemViewModel
        {
            ContactId = contact.ContactId,
            Name = contact.Name,
            CompanyName = contact.CompanyName,
            Email = contact.Email,
            Phone = contact.Phone,
            IsActive = contact.IsActive
        };
    }

    private static ContactDetailsViewModel MapToDetails(
        ContactData contact)
    {
        return new ContactDetailsViewModel
        {
            ContactId = contact.ContactId,
            Name = contact.Name,
            CompanyName = contact.CompanyName,
            Email = contact.Email,
            Phone = contact.Phone,
            Address = contact.Address,
            Website = contact.Website,
            Notes = contact.Notes,
            IsActive = contact.IsActive
        };
    }
}