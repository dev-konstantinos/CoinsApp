using CoinsApp.BLL.Features.Mints;
using CoinsApp.BLL.Features.Mints.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class MintMenu
{
    private readonly IMintService _mintService;

    public MintMenu(IMintService mintService)
    {
        _mintService =
            mintService
            ?? throw new ArgumentNullException(nameof(mintService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Mints ===");
            Console.WriteLine();
            Console.WriteLine("1. List Mints");
            Console.WriteLine("2. Mint Details");
            Console.WriteLine("3. Create Mint");
            Console.WriteLine("4. Update Mint");
            Console.WriteLine("5. Activate / Deactivate Mint");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await ListAsync();
                    break;

                case "2":
                    await DetailsAsync();
                    break;

                case "3":
                    await CreateAsync();
                    break;

                case "4":
                    await UpdateAsync();
                    break;

                case "5":
                    await SetActiveAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Pause();
                    break;
            }
        }
    }

    private async Task ListAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Mints ===");
        Console.WriteLine();

        try
        {
            var items = await _mintService.GetAllAsync();

            if (items.Count == 0)
            {
                Console.WriteLine("No mints found.");
                Pause();
                return;
            }

            Console.WriteLine(
                $"{"ID",4}  {"Country",-24} {"Name",-30} " +
                $"{"Code",-8} {"City",-18} {"Active",-7}");

            Console.WriteLine(new string('-', 91));

            foreach (var item in items)
            {
                var country =
                    $"{item.CountryName} ({item.CountryCode})";

                Console.WriteLine(
                    $"{item.MintId,4}  " +
                    $"{country,-24} " +
                    $"{item.Name,-30} " +
                    $"{item.Code ?? "-",-8} " +
                    $"{item.City ?? "-",-18} " +
                    $"{(item.IsActive ? "Yes" : "No"),-7}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading mints.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Mint Details ===");
        Console.WriteLine();

        var mintId =
            MenuInput.ReadRequiredId("Mint ID");

        try
        {
            var mint =
                await _mintService.GetByIdAsync(mintId);

            if (mint is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Mint with ID {mintId} was not found.");
            }
            else
            {
                Console.WriteLine();
                PrintDetails(mint);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading mint.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Mint ===");
        Console.WriteLine();

        try
        {
            var model = new CreateMintViewModel
            {
                CountryId =
                    MenuInput.ReadRequiredId("Country ID"),

                Name =
                    MenuInput.ReadRequiredString("Name"),

                Code =
                    MenuInput.ReadNullableString("Code"),

                City =
                    MenuInput.ReadNullableString("City"),

                IsActive =
                    MenuInput.ReadRequiredBoolean("Active")
            };

            var mint =
                await _mintService.CreateAsync(model);

            Console.WriteLine();

            if (mint is null)
            {
                Console.WriteLine(
                    "Mint could not be created.");
            }
            else
            {
                Console.WriteLine(
                    $"Mint created successfully. " +
                    $"Mint ID: {mint.MintId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating mint.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Mint ===");
        Console.WriteLine();

        var mintId =
            MenuInput.ReadRequiredId("Mint ID");

        try
        {
            var current =
                await _mintService.GetByIdAsync(mintId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Mint with ID {mintId} was not found.");

                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine(
                "Type null to clear optional values.");
            Console.WriteLine();

            var model = new UpdateMintViewModel
            {
                MintId =
                    current.MintId,

                CountryId =
                    MenuInput.ReadKeepCurrentId(
                        "Country ID",
                        current.CountryId),

                Name =
                    MenuInput.ReadKeepCurrentRequiredString(
                        "Name",
                        current.Name),

                Code =
                    MenuInput.ReadKeepCurrentString(
                        "Code",
                        current.Code),

                City =
                    MenuInput.ReadKeepCurrentString(
                        "City",
                        current.City),

                IsActive =
                    MenuInput.ReadKeepCurrentBoolean(
                        "Active",
                        current.IsActive)
            };

            var mint =
                await _mintService.UpdateAsync(model);

            Console.WriteLine();

            Console.WriteLine(
                mint is null
                    ? "Mint could not be updated."
                    : "Mint updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating mint.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task SetActiveAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Activate / Deactivate Mint ===");
        Console.WriteLine();

        var mintId =
            MenuInput.ReadRequiredId("Mint ID");

        try
        {
            var current =
                await _mintService.GetByIdAsync(mintId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Mint with ID {mintId} was not found.");

                Pause();
                return;
            }

            var newStatus =
                !current.IsActive;

            Console.WriteLine();
            Console.WriteLine($"Mint: {current.Name}");
            Console.WriteLine(
                $"Current status: " +
                $"{(current.IsActive ? "Active" : "Inactive")}");
            Console.WriteLine(
                $"New status: " +
                $"{(newStatus ? "Active" : "Inactive")}");
            Console.WriteLine();

            var confirmation =
                MenuInput.ReadRequiredString(
                    $"Type {(newStatus ? "ACTIVATE" : "DEACTIVATE")} to confirm");

            if (!string.Equals(
                    confirmation,
                    newStatus ? "ACTIVATE" : "DEACTIVATE",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Action cancelled.");

                Pause();
                return;
            }

            var mint =
                await _mintService.SetActiveAsync(
                    new SetMintActiveViewModel
                    {
                        MintId = mintId,
                        IsActive = newStatus
                    });

            Console.WriteLine();

            if (mint is null)
            {
                Console.WriteLine(
                    "Mint status could not be changed.");
            }
            else
            {
                Console.WriteLine(
                    newStatus
                        ? "Mint activated successfully."
                        : "Mint deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error changing mint status.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private static void PrintDetails(
        MintDetailsViewModel mint)
    {
        Console.WriteLine($"ID:       {mint.MintId}");
        Console.WriteLine(
            $"Country:  {mint.CountryName} " +
            $"({mint.CountryCode}) " +
            $"[ID {mint.CountryId}]");
        Console.WriteLine($"Name:     {mint.Name}");
        Console.WriteLine($"Code:     {mint.Code ?? "-"}");
        Console.WriteLine($"City:     {mint.City ?? "-"}");
        Console.WriteLine(
            $"Status:   " +
            $"{(mint.IsActive ? "Active" : "Inactive")}");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}