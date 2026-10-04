using CoinsApp.BLL.Features.Currencies;
using CoinsApp.BLL.Features.Currencies.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CurrencyMenu
{
    private readonly ICurrencyService _currencyService;

    public CurrencyMenu(ICurrencyService currencyService)
    {
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
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

            Console.WriteLine("1. List currencies");
            Console.WriteLine("2. Currency details");
            Console.WriteLine("3. Create currency");
            Console.WriteLine("4. Update currency");
            Console.WriteLine("5. Activate / Deactivate currency");
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
            var currencies = await _currencyService.GetAllAsync();

            PrintCurrencies(currencies);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading currencies.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Currency Details");
        Console.WriteLine();

        var currencyId = MenuInput.ReadIdOrExit("Currency ID");

        if (currencyId is null)
        {
            return;
        }

        try
        {
            var currency = await _currencyService.GetByIdAsync(currencyId.Value);

            if (currency is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Currency with ID {currencyId.Value} was not found.");
            }
            else
            {
                PrintDetails(currency);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading currency.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create Currency");
        Console.WriteLine();

        try
        {
            var model = new CreateCurrencyViewModel
            {
                Name = MenuInput.ReadRequiredString("Name"),
                Code = MenuInput.ReadRequiredString("Code"),
                Symbol = MenuInput.ReadNullableString("Symbol"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating currency...");

            var currency = await _currencyService.CreateAsync(model);

            if (currency is null)
            {
                Console.WriteLine();
                Console.WriteLine("Currency could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Currency created successfully.");
                Console.WriteLine($"Currency ID: {currency.CurrencyId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating currency.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update Currency");
        Console.WriteLine();

        var currencyId = MenuInput.ReadIdOrExit("Currency ID");

        if (currencyId is null)
        {
            return;
        }

        try
        {
            var current = await _currencyService.GetByIdAsync(currencyId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Currency with ID {currencyId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update Currency");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine("Type null for Symbol to clear it.");
            Console.WriteLine();

            var model = new UpdateCurrencyViewModel
            {
                CurrencyId = current.CurrencyId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                Code = MenuInput.ReadKeepCurrentRequiredString("Code", current.Code),
                Symbol = MenuInput.ReadKeepCurrentString("Symbol", current.Symbol),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating currency...");

            var currency = await _currencyService.UpdateAsync(model);

            if (currency is null)
            {
                Console.WriteLine();
                Console.WriteLine("Currency could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Currency updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating currency.");
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
        Console.WriteLine("Activate / Deactivate Currency");
        Console.WriteLine();

        var currencyId = MenuInput.ReadIdOrExit("Currency ID");

        if (currencyId is null)
        {
            return;
        }

        try
        {
            var current = await _currencyService.GetByIdAsync(currencyId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Currency with ID {currencyId.Value} was not found.");

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

            var model = new SetCurrencyActiveViewModel
            {
                CurrencyId = current.CurrencyId,
                IsActive = newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating currency..."
                    : "Deactivating currency...");

            var currency = await _currencyService.SetActiveAsync(model);

            if (currency is null)
            {
                Console.WriteLine();
                Console.WriteLine("Currency status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Currency activated successfully."
                        : "Currency deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing currency status.");
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
        Console.WriteLine("=== Currencies ===");
        Console.WriteLine();
    }

    private static void PrintCurrencies(IReadOnlyList<CurrencyListItemViewModel> currencies)
    {
        if (currencies.Count == 0)
        {
            Console.WriteLine("No currencies found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Name",-30} " +
            $"{"Code",-10} " +
            $"{"Symbol",-10} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 67));

        foreach (var currency in currencies)
        {
            Console.WriteLine(
                $"{currency.CurrencyId,4}  " +
                $"{currency.Name,-30} " +
                $"{currency.Code,-10} " +
                $"{currency.Symbol ?? "-",-10} " +
                $"{(currency.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(CurrencyDetailsViewModel currency)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Currency ID: {currency.CurrencyId}");
        Console.WriteLine($"Name:        {currency.Name}");
        Console.WriteLine($"Code:        {currency.Code}");

        Console.WriteLine();

        Console.WriteLine("--- Symbol ---");
        Console.WriteLine($"Symbol:      {currency.Symbol ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:      {(currency.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(UpdateCurrencyViewModel currency)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Currency ID: {currency.CurrencyId}");
        Console.WriteLine($"Name:        {currency.Name}");
        Console.WriteLine($"Code:        {currency.Code}");

        Console.WriteLine();

        Console.WriteLine("--- Symbol ---");
        Console.WriteLine($"Symbol:      {currency.Symbol ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:      {(currency.IsActive ? "Yes" : "No")}");
    }

    private static void PrintStatusChange(CurrencyDetailsViewModel currency)
    {
        Console.WriteLine("--- Currency ---");
        Console.WriteLine($"Currency ID: {currency.CurrencyId}");
        Console.WriteLine($"Name:        {currency.Name}");
        Console.WriteLine($"Code:        {currency.Code}");
        Console.WriteLine($"Symbol:      {currency.Symbol ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Current Status ---");
        Console.WriteLine($"Active:      {(currency.IsActive ? "Yes" : "No")}");
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