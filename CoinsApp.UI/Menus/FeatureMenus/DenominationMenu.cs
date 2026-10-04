using CoinsApp.BLL.Features.Denominations;
using CoinsApp.BLL.Features.Denominations.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class DenominationMenu
{
    private readonly IDenominationService _denominationService;

    public DenominationMenu(IDenominationService denominationService)
    {
        _denominationService = denominationService ?? throw new ArgumentNullException(nameof(denominationService));
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

            Console.WriteLine("1. List denominations");
            Console.WriteLine("2. Denomination details");
            Console.WriteLine("3. List denominations by currency");
            Console.WriteLine("4. Create denomination");
            Console.WriteLine("5. Update denomination");
            Console.WriteLine("6. Activate / Deactivate denomination");
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
                    await ListByCurrencyAsync();
                    break;

                case 4:
                    await CreateAsync();
                    break;

                case 5:
                    await UpdateAsync();
                    break;

                case 6:
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
            var denominations = await _denominationService.GetAllAsync();

            PrintDenominations(denominations);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading denominations.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Denomination Details");
        Console.WriteLine();

        var denominationId = MenuInput.ReadIdOrExit("Denomination ID");

        if (denominationId is null)
        {
            return;
        }

        try
        {
            var denomination = await _denominationService.GetByIdAsync(denominationId.Value);

            if (denomination is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Denomination with ID {denominationId.Value} was not found.");
            }
            else
            {
                PrintDetails(denomination);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading denomination.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task ListByCurrencyAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Denominations by Currency");
        Console.WriteLine();

        var currencyId = MenuInput.ReadIdOrExit("Currency ID");

        if (currencyId is null)
        {
            return;
        }

        try
        {
            var denominations = await _denominationService.GetByCurrencyAsync(currencyId.Value);

            PrintDenominations(denominations);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading denominations for currency.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create Denomination");
        Console.WriteLine();

        try
        {
            var model = new CreateDenominationViewModel
            {
                CurrencyId = MenuInput.ReadRequiredId("Currency ID"),
                Value = MenuInput.ReadRequiredPrice("Value"),
                DisplayName = MenuInput.ReadRequiredString("Display name"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating denomination...");

            var denomination = await _denominationService.CreateAsync(model);

            if (denomination is null)
            {
                Console.WriteLine();
                Console.WriteLine("Denomination could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Denomination created successfully.");
                Console.WriteLine($"Denomination ID: {denomination.DenominationId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating denomination.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update Denomination");
        Console.WriteLine();

        var denominationId = MenuInput.ReadIdOrExit("Denomination ID");

        if (denominationId is null)
        {
            return;
        }

        try
        {
            var current = await _denominationService.GetByIdAsync(denominationId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Denomination with ID {denominationId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update Denomination");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateDenominationViewModel
            {
                DenominationId = current.DenominationId,
                CurrencyId = MenuInput.ReadKeepCurrentId("Currency ID", current.CurrencyId),
                Value = MenuInput.ReadKeepCurrentPrice("Value", current.Value),
                DisplayName = MenuInput.ReadKeepCurrentRequiredString("Display name", current.DisplayName),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating denomination...");

            var denomination = await _denominationService.UpdateAsync(model);

            if (denomination is null)
            {
                Console.WriteLine();
                Console.WriteLine("Denomination could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Denomination updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating denomination.");
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
        Console.WriteLine("Activate / Deactivate Denomination");
        Console.WriteLine();

        var denominationId = MenuInput.ReadIdOrExit("Denomination ID");

        if (denominationId is null)
        {
            return;
        }

        try
        {
            var current = await _denominationService.GetByIdAsync(denominationId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Denomination with ID {denominationId.Value} was not found.");

                Pause();
                return;
            }

            PrintStatusChange(current);

            var newStatus = !current.IsActive;
            var expectedConfirmation = newStatus ? "ACTIVATE" : "DEACTIVATE";

            Console.WriteLine();
            Console.WriteLine($"New status: {(newStatus ? "Active" : "Inactive")}");

            Console.WriteLine();

            var confirmation = MenuInput.ReadRequiredString($"Type {expectedConfirmation} to confirm");

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

            var model = new SetDenominationActiveViewModel
            {
                DenominationId = current.DenominationId,
                IsActive = newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating denomination..."
                    : "Deactivating denomination...");

            var denomination = await _denominationService.SetActiveAsync(model);

            if (denomination is null)
            {
                Console.WriteLine();
                Console.WriteLine("Denomination status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Denomination activated successfully."
                        : "Denomination deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing denomination status.");
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
        Console.WriteLine("=== Denominations ===");
        Console.WriteLine();
    }

    private static void PrintDenominations(IReadOnlyList<DenominationListItemViewModel> denominations)
    {
        if (denominations.Count == 0)
        {
            Console.WriteLine("No denominations found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Currency",-12} " +
            $"{"Value",12} " +
            $"{"Display Name",-20} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 63));

        foreach (var denomination in denominations)
        {
            Console.WriteLine(
                $"{denomination.DenominationId,4}  " +
                $"{denomination.CurrencyCode,-12} " +
                $"{denomination.Value,12:0.####} " +
                $"{denomination.DisplayName,-20} " +
                $"{(denomination.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(
        DenominationDetailsViewModel denomination)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Denomination ID: {denomination.DenominationId}");

        Console.WriteLine();

        Console.WriteLine("--- Currency ---");
        Console.WriteLine($"Currency ID:   {denomination.CurrencyId}");

        Console.WriteLine($"Currency:      {denomination.CurrencyCode}");

        Console.WriteLine();

        Console.WriteLine("--- Denomination ---");
        Console.WriteLine($"Value:         {denomination.Value:0.####}");

        Console.WriteLine($"Display Name:  {denomination.DisplayName}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(denomination.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(
        UpdateDenominationViewModel denomination)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Denomination ID: {denomination.DenominationId}");

        Console.WriteLine();

        Console.WriteLine("--- Currency ---");
        Console.WriteLine($"Currency ID:   {denomination.CurrencyId}");

        Console.WriteLine();

        Console.WriteLine("--- Denomination ---");
        Console.WriteLine($"Value:         {denomination.Value:0.####}");

        Console.WriteLine($"Display Name:  {denomination.DisplayName}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(denomination.IsActive ? "Yes" : "No")}");
    }

    private static void PrintStatusChange(
        DenominationDetailsViewModel denomination)
    {
        Console.WriteLine("--- Denomination ---");
        Console.WriteLine($"Denomination ID: {denomination.DenominationId}");

        Console.WriteLine($"Currency:        {denomination.CurrencyCode}");

        Console.WriteLine($"Value:           {denomination.Value:0.####}");

        Console.WriteLine($"Display Name:    {denomination.DisplayName}");

        Console.WriteLine();

        Console.WriteLine("--- Current Status ---");
        Console.WriteLine($"Active:          {(denomination.IsActive ? "Yes" : "No")}");
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