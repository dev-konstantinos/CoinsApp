using CoinsApp.BLL.Features.Mints;
using CoinsApp.BLL.Features.Mints.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class MintMenu
{
    private readonly IMintService _mintService;

    public MintMenu(IMintService mintService)
    {
        _mintService = mintService ?? throw new ArgumentNullException(nameof(mintService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            PrintHeader();

            Console.WriteLine("1. List mints");
            Console.WriteLine("2. Mint details");
            Console.WriteLine("3. Create mint");
            Console.WriteLine("4. Update mint");
            Console.WriteLine("5. Activate / Deactivate mint");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Pause();
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
                    await SetActiveAsync();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine("Press Enter to continue...");
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

        PrintHeader();

        try
        {
            var mints = await _mintService.GetAllAsync();

            PrintMints(mints);
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

        PrintHeader();
        Console.WriteLine("Mint Details");
        Console.WriteLine();

        var mintId = MenuInput.ReadIdOrExit("Mint ID");

        if (mintId is null)
        {
            return;
        }

        try
        {
            var mint = await _mintService.GetByIdAsync(mintId.Value);

            if (mint is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Mint with ID {mintId.Value} was not found.");
            }
            else
            {
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

        PrintHeader();
        Console.WriteLine("Create Mint");
        Console.WriteLine();

        try
        {
            var model = new CreateMintViewModel
            {
                CountryId = MenuInput.ReadRequiredId("Country ID"),
                Name = MenuInput.ReadRequiredString("Name"),
                Code = MenuInput.ReadNullableString("Code"),
                City = MenuInput.ReadNullableString("City"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating mint...");

            var mint = await _mintService.CreateAsync(model);

            if (mint is null)
            {
                Console.WriteLine();
                Console.WriteLine("Mint could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Mint created successfully.");
                Console.WriteLine($"Mint ID: {mint.MintId}");
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

        PrintHeader();
        Console.WriteLine("Update Mint");
        Console.WriteLine();

        var mintId = MenuInput.ReadIdOrExit("Mint ID");

        if (mintId is null)
        {
            return;
        }

        try
        {
            var current = await _mintService.GetByIdAsync(mintId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Mint with ID {mintId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update Mint");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine("Type null to clear optional values.");
            Console.WriteLine();

            var model = new UpdateMintViewModel
            {
                MintId = current.MintId,
                CountryId = MenuInput.ReadKeepCurrentId("Country ID", current.CountryId),
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                Code = MenuInput.ReadKeepCurrentString("Code", current.Code),
                City = MenuInput.ReadKeepCurrentString("City", current.City),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating mint...");

            var mint = await _mintService.UpdateAsync(model);

            if (mint is null)
            {
                Console.WriteLine();
                Console.WriteLine("Mint could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Mint updated successfully.");
            }
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

    // ============================================================
    // Domain actions
    // ============================================================

    private async Task SetActiveAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Activate / Deactivate Mint");
        Console.WriteLine();

        var mintId = MenuInput.ReadIdOrExit("Mint ID");

        if (mintId is null)
        {
            return;
        }

        try
        {
            var current = await _mintService.GetByIdAsync(mintId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Mint with ID {mintId.Value} was not found.");

                Pause();
                return;
            }

            PrintStatusChange(current);

            var newStatus = !current.IsActive;
            var expectedConfirmation = newStatus ? "ACTIVATE" : "DEACTIVATE";

            Console.WriteLine();
            Console.WriteLine($"New status: {(newStatus ? "Active" : "Inactive")}");

            Console.WriteLine();

            var confirmation =
                MenuInput.ReadRequiredString($"Type {expectedConfirmation} to confirm");

            if (!string.Equals(
                    confirmation,
                    expectedConfirmation,
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Action cancelled.");

                Pause();
                return;
            }

            var model = new SetMintActiveViewModel
            {
                MintId = current.MintId,
                IsActive = newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating mint..."
                    : "Deactivating mint...");

            var mint = await _mintService.SetActiveAsync(model);

            if (mint is null)
            {
                Console.WriteLine();
                Console.WriteLine("Mint status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Mint activated successfully."
                        : "Mint deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing mint status.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine("=== Mints ===");
        Console.WriteLine();
    }

    private static void PrintMints(IReadOnlyList<MintListItemViewModel> mints)
    {
        if (mints.Count == 0)
        {
            Console.WriteLine("No mints found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Country",-24} " +
            $"{"Name",-30} " +
            $"{"Code",-8} " +
            $"{"City",-18} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 91));

        foreach (var mint in mints)
        {
            var country = $"{mint.CountryName} ({mint.CountryCode})";

            Console.WriteLine(
                $"{mint.MintId,4}  " +
                $"{country,-24} " +
                $"{mint.Name,-30} " +
                $"{mint.Code ?? "-",-8} " +
                $"{mint.City ?? "-",-18} " +
                $"{(mint.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(
        MintDetailsViewModel mint)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Mint ID: {mint.MintId}");

        Console.WriteLine();

        Console.WriteLine("--- Country ---");
        Console.WriteLine($"Country ID: {mint.CountryId}");
        Console.WriteLine($"Country:    {mint.CountryName} ({mint.CountryCode})");

        Console.WriteLine();

        Console.WriteLine("--- Mint ---");
        Console.WriteLine($"Name:       {mint.Name}");
        Console.WriteLine($"Code:       {mint.Code ?? "-"}");
        Console.WriteLine($"City:       {mint.City ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:     {(mint.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(
        UpdateMintViewModel mint)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Mint ID: {mint.MintId}");

        Console.WriteLine();

        Console.WriteLine("--- Country ---");
        Console.WriteLine($"Country ID: {mint.CountryId}");

        Console.WriteLine();

        Console.WriteLine("--- Mint ---");
        Console.WriteLine($"Name:       {mint.Name}");
        Console.WriteLine($"Code:       {mint.Code ?? "-"}");
        Console.WriteLine($"City:       {mint.City ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:     {(mint.IsActive ? "Yes" : "No")}");
    }

    private static void PrintStatusChange(
        MintDetailsViewModel mint)
    {
        Console.WriteLine("--- Mint ---");
        Console.WriteLine($"Mint ID:    {mint.MintId}");
        Console.WriteLine($"Country:    {mint.CountryName} ({mint.CountryCode})");
        Console.WriteLine($"Name:       {mint.Name}");
        Console.WriteLine($"Code:       {mint.Code ?? "-"}");
        Console.WriteLine($"City:       {mint.City ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Current Status ---");
        Console.WriteLine($"Active:     {(mint.IsActive ? "Yes" : "No")}");
    }

    // ============================================================
    // General helpers
    // ============================================================

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}