using CoinsApp.BLL.Denominations;
using CoinsApp.BLL.Denominations.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus;

internal sealed class DenominationMenu
{
    private readonly DenominationService _denominationService;

    public DenominationMenu(DenominationService denominationService)
    {
        _denominationService =
            denominationService
            ?? throw new ArgumentNullException(nameof(denominationService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Denominations ===");
            Console.WriteLine();
            Console.WriteLine("1. List denominations");
            Console.WriteLine("2. Denomination details");
            Console.WriteLine("3. Create denomination");
            Console.WriteLine("4. Update denomination");
            Console.WriteLine("5. Activate / Deactivate denomination");
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

        Console.WriteLine("=== Denominations ===");
        Console.WriteLine();

        try
        {
            var items = await _denominationService.GetAllAsync();

            if (items.Count == 0)
            {
                Console.WriteLine("No denominations found.");
                Pause();
                return;
            }

            Console.WriteLine(
                $"{"ID",4}  {"Currency",-24} {"Value",-12} " +
                $"{"Display Name",-24} {"Active",-7}");

            Console.WriteLine(new string('-', 83));

            foreach (var item in items)
            {
                var currency =
                    $"{item.CurrencyName} ({item.CurrencyCode})";

                Console.WriteLine(
                    $"{item.DenominationId,4}  " +
                    $"{currency,-24} " +
                    $"{item.Value,12:0.####} " +
                    $"{item.DisplayName,-24} " +
                    $"{(item.IsActive ? "Yes" : "No"),-7}");
            }
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

        Console.WriteLine("=== Denomination Details ===");
        Console.WriteLine();

        var denominationId =
            MenuInput.ReadRequiredId("Denomination ID");

        try
        {
            var denomination =
                await _denominationService.GetByIdAsync(denominationId);

            if (denomination is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Denomination with ID {denominationId} was not found.");
            }
            else
            {
                Console.WriteLine();
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

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Denomination ===");
        Console.WriteLine();

        try
        {
            var model = new CreateDenominationViewModel
            {
                CurrencyId =
                    MenuInput.ReadRequiredId("Currency ID"),

                Value =
                    MenuInput.ReadRequiredPrice("Value"),

                DisplayName =
                    MenuInput.ReadRequiredString("Display Name"),

                IsActive =
                    MenuInput.ReadRequiredBoolean("Active")
            };

            var denomination =
                await _denominationService.CreateAsync(model);

            Console.WriteLine();

            if (denomination is null)
            {
                Console.WriteLine(
                    "Denomination could not be created.");
            }
            else
            {
                Console.WriteLine(
                    $"Denomination created successfully. " +
                    $"Denomination ID: {denomination.DenominationId}");
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

        Console.WriteLine("=== Update Denomination ===");
        Console.WriteLine();

        var denominationId =
            MenuInput.ReadRequiredId("Denomination ID");

        try
        {
            var current =
                await _denominationService.GetByIdAsync(denominationId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Denomination with ID {denominationId} was not found.");

                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateDenominationViewModel
            {
                DenominationId =
                    current.DenominationId,

                CurrencyId =
                    MenuInput.ReadKeepCurrentId(
                        "Currency ID",
                        current.CurrencyId),

                Value =
                    MenuInput.ReadKeepCurrentPrice(
                        "Value",
                        current.Value),

                DisplayName =
                    MenuInput.ReadKeepCurrentRequiredString(
                        "Display Name",
                        current.DisplayName),

                IsActive =
                    MenuInput.ReadKeepCurrentBoolean(
                        "Active",
                        current.IsActive)
            };

            var denomination =
                await _denominationService.UpdateAsync(model);

            Console.WriteLine();

            Console.WriteLine(
                denomination is null
                    ? "Denomination could not be updated."
                    : "Denomination updated successfully.");
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

    private async Task SetActiveAsync()
    {
        Console.Clear();

        Console.WriteLine(
            "=== Activate / Deactivate Denomination ===");
        Console.WriteLine();

        var denominationId =
            MenuInput.ReadRequiredId("Denomination ID");

        try
        {
            var current =
                await _denominationService.GetByIdAsync(denominationId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Denomination with ID {denominationId} was not found.");

                Pause();
                return;
            }

            var newStatus =
                !current.IsActive;

            Console.WriteLine();
            Console.WriteLine(
                $"Denomination: {current.DisplayName}");
            Console.WriteLine(
                $"Currency: {current.CurrencyName} " +
                $"({current.CurrencyCode})");
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
                Console.WriteLine("Action was cancelled.");

                Pause();
                return;
            }

            var denomination =
                await _denominationService.SetActiveAsync(
                    new SetDenominationActiveViewModel
                    {
                        DenominationId = current.DenominationId,
                        IsActive = newStatus
                    });

            Console.WriteLine();

            if (denomination is null)
            {
                Console.WriteLine(
                    "Denomination status could not be changed.");
            }
            else
            {
                Console.WriteLine(
                    newStatus
                        ? "Denomination activated successfully."
                        : "Denomination deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error changing denomination status.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private static void PrintDetails(
        DenominationDetailsViewModel denomination)
    {
        Console.WriteLine(
            $"Denomination ID: {denomination.DenominationId}");

        Console.WriteLine(
            $"Currency: {denomination.CurrencyName} " +
            $"({denomination.CurrencyCode}) " +
            $"[ID {denomination.CurrencyId}]");

        Console.WriteLine(
            $"Value: {denomination.Value:0.####}");

        Console.WriteLine(
            $"Display Name: {denomination.DisplayName}");

        Console.WriteLine(
            $"Active: {(denomination.IsActive ? "Yes" : "No")}");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
