using CoinsApp.BLL.Contacts;
using CoinsApp.BLL.Contacts.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus;

internal sealed class ContactMenu
{
    private readonly ContactService _contactService;

    public ContactMenu(ContactService contactService)
    {
        _contactService =
            contactService
            ?? throw new ArgumentNullException(nameof(contactService));
    }

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

            var input =
                Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.ReadLine();
                continue;
            }

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
    private async Task ListAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Contacts ===");
        Console.WriteLine();

        var contacts =
            await _contactService.GetAllAsync();

        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            Console.ReadLine();
            return;
        }

        foreach (var contact in contacts)
        {
            Console.WriteLine(
                $"{contact.ContactId}: {contact.Name}");

            if (!string.IsNullOrWhiteSpace(contact.CompanyName))
            {
                Console.WriteLine(
                    $"   Company: {contact.CompanyName}");
            }

            if (!string.IsNullOrWhiteSpace(contact.Email))
            {
                Console.WriteLine(
                    $"   Email: {contact.Email}");
            }

            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                Console.WriteLine(
                    $"   Phone: {contact.Phone}");
            }

            Console.WriteLine(
                $"   Status: {(contact.IsActive ? "Active" : "Inactive")}");

            Console.WriteLine();
        }

        Console.ReadLine();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Contact Details ===");
        Console.WriteLine();

        var contactId =
            MenuInput.ReadRequiredId("Contact ID");

        var contact =
            await _contactService.GetByIdAsync(contactId);

        if (contact is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Contact with ID {contactId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"ID:          {contact.ContactId}");

        Console.WriteLine(
            $"Name:        {contact.Name}");

        Console.WriteLine(
            $"Company:     {contact.CompanyName ?? "-"}");

        Console.WriteLine(
            $"Email:       {contact.Email ?? "-"}");

        Console.WriteLine(
            $"Phone:       {contact.Phone ?? "-"}");

        Console.WriteLine(
            $"Address:     {contact.Address ?? "-"}");

        Console.WriteLine(
            $"Website:     {contact.Website ?? "-"}");

        Console.WriteLine(
            $"Notes:       {contact.Notes ?? "-"}");

        Console.WriteLine(
            $"Status:      {(contact.IsActive ? "Active" : "Inactive")}");

        Console.ReadLine();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Contact ===");
        Console.WriteLine();

        var model = new CreateContactViewModel
        {
            Name =
                MenuInput.ReadRequiredString("Name"),

            CompanyName =
                MenuInput.ReadNullableString("Company"),

            Email =
                MenuInput.ReadNullableString("Email"),

            Phone =
                MenuInput.ReadNullableString("Phone"),

            Address =
                MenuInput.ReadNullableString("Address"),

            Website =
                MenuInput.ReadNullableString("Website"),

            Notes =
                MenuInput.ReadNullableString("Notes"),

            IsActive =
                MenuInput.ReadRequiredBoolean("Active")
        };

        var contactId =
            await _contactService.CreateAsync(model);

        Console.WriteLine();
        Console.WriteLine(
            $"Contact created successfully. ID: {contactId}");

        Console.ReadLine();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Contact ===");
        Console.WriteLine();

        var contactId =
            MenuInput.ReadRequiredId("Contact ID");

        var current =
            await _contactService.GetByIdAsync(contactId);

        if (current is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Contact with ID {contactId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Enter the new values.");
        Console.WriteLine(
            "Press Enter to keep the current value.");
        Console.WriteLine();

        var name =
            ReadKeepCurrentRequiredString(
                "Name",
                current.Name);

        var model = new UpdateContactViewModel
        {
            ContactId = contactId,

            Name = name,

            CompanyName =
                MenuInput.ReadKeepCurrentString(
                    "Company",
                    current.CompanyName),

            Email =
                MenuInput.ReadKeepCurrentString(
                    "Email",
                    current.Email),

            Phone =
                MenuInput.ReadKeepCurrentString(
                    "Phone",
                    current.Phone),

            Address =
                MenuInput.ReadKeepCurrentString(
                    "Address",
                    current.Address),

            Website =
                MenuInput.ReadKeepCurrentString(
                    "Website",
                    current.Website),

            Notes =
                MenuInput.ReadKeepCurrentString(
                    "Notes",
                    current.Notes),

            IsActive =
                MenuInput.ReadKeepCurrentBoolean(
                    "Active",
                    current.IsActive)
        };

        var updated =
            await _contactService.UpdateAsync(model);

        Console.WriteLine();

        if (updated is null)
        {
            Console.WriteLine(
                $"Contact with ID {contactId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"Contact {updated.ContactId} updated successfully.");

        Console.ReadLine();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Contact ===");
        Console.WriteLine();

        var contactId =
            MenuInput.ReadRequiredId("Contact ID");

        var current =
            await _contactService.GetByIdAsync(contactId);

        if (current is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Contact with ID {contactId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Contact: {current.Name}");

        if (!string.IsNullOrWhiteSpace(current.CompanyName))
        {
            Console.WriteLine(
                $"Company: {current.CompanyName}");
        }

        Console.WriteLine();

        var confirmed =
            MenuInput.ReadRequiredBoolean(
                "Delete this contact");

        if (!confirmed)
        {
            Console.WriteLine();
            Console.WriteLine("Delete cancelled.");
            Console.ReadLine();
            return;
        }

        await _contactService.DeleteAsync(
            new DeleteContactViewModel
            {
                ContactId = contactId
            });

        Console.WriteLine();
        Console.WriteLine(
            $"Contact {contactId} deleted successfully.");

        Console.ReadLine();
    }

    private static string ReadKeepCurrentRequiredString(
        string label,
        string current)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{current}] " +
                "(Enter = keep current): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            return input;
        }
    }
}