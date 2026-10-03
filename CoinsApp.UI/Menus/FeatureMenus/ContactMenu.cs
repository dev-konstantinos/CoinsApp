using CoinsApp.BLL.Features.Contacts;
using CoinsApp.BLL.Features.Contacts.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class ContactMenu
{
    private readonly IContactService _contactService;

    public ContactMenu(IContactService contactService)
    {
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Contacts ===");
            Console.WriteLine();
            Console.WriteLine("1. List Contacts");
            Console.WriteLine("2. Contact Details");
            Console.WriteLine("3. Create Contact");
            Console.WriteLine("4. Update Contact");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    await ListAsync();
                    break;

                case 2:
                    await DetailsAsync();
                    break;

                case 3:
                    await CreateAsync();
                    break;

                case 4:
                    await UpdateAsync();
                    break;

                case 5:
                    await DeleteAsync();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    // ============================================================
    // CRUD
    // ============================================================

    private async Task ListAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Contacts ===");
        Console.WriteLine();

        try
        {
            var contacts = await _contactService.GetAllAsync();

            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintHeader();

            foreach (var contact in contacts)
            {
                PrintContact(contact);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading contacts.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Contact Details ===");
        Console.WriteLine();

        var contactId = MenuInput.ReadIdOrExit("Contact ID");

        if (contactId is null)
            return;

        try
        {
            var contact = await _contactService.GetByIdAsync(contactId.Value);

            if (contact is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Contact with ID {contactId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            PrintDetails(contact);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading contact.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Contact ===");
        Console.WriteLine();

        try
        {
            var model = new CreateContactViewModel
            {
                Name = MenuInput.ReadRequiredString("Name"),
                CompanyName = MenuInput.ReadNullableString("Company"),
                Email = MenuInput.ReadNullableString("Email"),
                Phone = MenuInput.ReadNullableString("Phone"),
                Address = MenuInput.ReadNullableString("Address"),
                Website = MenuInput.ReadNullableString("Website"),
                Notes = MenuInput.ReadNullableString("Notes"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            var contactId = await _contactService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine($"Contact created successfully. Contact ID: {contactId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating contact.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Contact ===");
        Console.WriteLine();

        var contactId = MenuInput.ReadIdOrExit("Contact ID");

        if (contactId is null)
            return;

        try
        {
            var contact = await _contactService.GetByIdAsync(contactId.Value);

            if (contact is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Contact with ID {contactId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Enter the new values.");
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateContactViewModel
            {
                ContactId = contact.ContactId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", contact.Name),
                CompanyName = MenuInput.ReadKeepCurrentString("Company", contact.CompanyName),
                Email = MenuInput.ReadKeepCurrentString("Email", contact.Email),
                Phone = MenuInput.ReadKeepCurrentString("Phone", contact.Phone),
                Address = MenuInput.ReadKeepCurrentString("Address", contact.Address),
                Website = MenuInput.ReadKeepCurrentString("Website", contact.Website),
                Notes = MenuInput.ReadKeepCurrentString("Notes", contact.Notes),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", contact.IsActive)
            };

            Console.WriteLine();
            Console.WriteLine("=== Update Preview ===");
            Console.WriteLine();

            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.Write("Save changes? (y/n): ");

            var confirmation = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirmation != "y" && confirmation != "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Update cancelled.");
                Console.ReadLine();
                return;
            }

            var updated = await _contactService.UpdateAsync(model);

            Console.WriteLine();

            if (updated is null)
            {
                Console.WriteLine($"Contact with ID {contactId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Contact {updated.ContactId} updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating contact.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Contact ===");
        Console.WriteLine();

        var contactId = MenuInput.ReadIdOrExit("Contact ID");

        if (contactId is null)
            return;

        try
        {
            var contact = await _contactService.GetByIdAsync(contactId.Value);

            if (contact is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Contact with ID {contactId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            PrintDetails(contact);

            Console.WriteLine();
            Console.WriteLine("=== WARNING ===");
            Console.WriteLine();
            Console.WriteLine("Deleting this contact is permanent.");
            Console.WriteLine();

            Console.Write("Type DELETE to confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(confirmation, "DELETE", StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Console.ReadLine();
                return;
            }

            await _contactService.DeleteAsync(new DeleteContactViewModel
            {
                ContactId = contact.ContactId
            });

            Console.WriteLine();
            Console.WriteLine($"Contact {contactId.Value} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting contact.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine(
            $"{"ID",4}  {"Name",-28} {"Company",-28} {"Email",-34} {"Phone",-18} {"Status",-10}");

        Console.WriteLine(new string('-', 132));
    }

    private static void PrintContact(ContactListItemViewModel contact)
    {
        Console.WriteLine(
            $"{contact.ContactId,4}  {Truncate(contact.Name, 28),-28} {Truncate(contact.CompanyName, 28),-28} {Truncate(contact.Email, 34),-34} {Truncate(contact.Phone, 18),-18} {(contact.IsActive ? "Active" : "Inactive"),-10}");
    }

    private static void PrintDetails(ContactDetailsViewModel contact)
    {
        Console.WriteLine("--- Contact ---");
        Console.WriteLine($"Contact ID:     {contact.ContactId}");
        Console.WriteLine($"Name:           {contact.Name}");
        Console.WriteLine($"Company:        {contact.CompanyName ?? "-"}");

        Console.WriteLine();
        Console.WriteLine("--- Contact Information ---");
        Console.WriteLine($"Email:          {contact.Email ?? "-"}");
        Console.WriteLine($"Phone:          {contact.Phone ?? "-"}");
        Console.WriteLine($"Address:        {contact.Address ?? "-"}");
        Console.WriteLine($"Website:        {contact.Website ?? "-"}");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Status:         {(contact.IsActive ? "Active" : "Inactive")}");

        Console.WriteLine();
        Console.WriteLine("--- Notes ---");
        Console.WriteLine(contact.Notes ?? "-");
    }

    private static void PrintUpdateSummary(UpdateContactViewModel model)
    {
        Console.WriteLine("--- Contact ---");
        Console.WriteLine($"Contact ID:     {model.ContactId}");
        Console.WriteLine($"Name:           {model.Name}");
        Console.WriteLine($"Company:        {model.CompanyName ?? "null"}");

        Console.WriteLine();
        Console.WriteLine("--- Contact Information ---");
        Console.WriteLine($"Email:          {model.Email ?? "null"}");
        Console.WriteLine($"Phone:          {model.Phone ?? "null"}");
        Console.WriteLine($"Address:        {model.Address ?? "null"}");
        Console.WriteLine($"Website:        {model.Website ?? "null"}");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Status:         {(model.IsActive ? "Active" : "Inactive")}");

        Console.WriteLine();
        Console.WriteLine("--- Notes ---");
        Console.WriteLine(model.Notes ?? "null");
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "-";

        value = value.Trim();

        if (value.Length <= maxLength)
            return value;

        return value[..(maxLength - 3)] + "...";
    }
}