using CoinsApp.BLL.Features.Countries;
using CoinsApp.BLL.Features.Countries.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CountryMenu
{
    private readonly ICountryService _countryService;

    public CountryMenu(ICountryService countryService)
    {
        _countryService =
            countryService
            ?? throw new ArgumentNullException(nameof(countryService));
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

            Console.WriteLine("1. List countries");
            Console.WriteLine("2. Country details");
            Console.WriteLine("3. Create country");
            Console.WriteLine("4. Update country");
            Console.WriteLine("5. Activate / Deactivate country");
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
            var countries = await _countryService.GetAllAsync();

            PrintCountries(countries);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading countries.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Country Details");
        Console.WriteLine();

        var countryId = MenuInput.ReadIdOrExit("Country ID");

        if (countryId is null)
        {
            return;
        }

        try
        {
            var country = await _countryService.GetByIdAsync(countryId.Value);

            if (country is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Country with ID {countryId.Value} was not found.");
            }
            else
            {
                PrintDetails(country);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading country.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create Country");
        Console.WriteLine();

        try
        {
            var model = new CreateCountryViewModel
            {
                Name = MenuInput.ReadRequiredString("Name"),
                Code = MenuInput.ReadRequiredString("Code"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating country...");

            var country = await _countryService.CreateAsync(model);

            if (country is null)
            {
                Console.WriteLine();
                Console.WriteLine("Country could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Country created successfully.");
                Console.WriteLine($"Country ID: {country.CountryId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating country.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update Country");
        Console.WriteLine();

        var countryId = MenuInput.ReadIdOrExit("Country ID");

        if (countryId is null)
        {
            return;
        }

        try
        {
            var current = await _countryService.GetByIdAsync(countryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Country with ID {countryId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update Country");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCountryViewModel
            {
                CountryId = current.CountryId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                Code = MenuInput.ReadKeepCurrentRequiredString("Code", current.Code),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating country...");

            var country = await _countryService.UpdateAsync(model);

            if (country is null)
            {
                Console.WriteLine();
                Console.WriteLine("Country could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Country updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating country.");
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
        Console.WriteLine("Activate / Deactivate Country");
        Console.WriteLine();

        var countryId = MenuInput.ReadIdOrExit("Country ID");

        if (countryId is null)
        {
            return;
        }

        try
        {
            var current = await _countryService.GetByIdAsync(countryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Country with ID {countryId.Value} was not found.");

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

            var model = new SetCountryActiveViewModel
            {
                CountryId = current.CountryId,
                IsActive = newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating country..."
                    : "Deactivating country...");

            var country = await _countryService.SetActiveAsync(model);

            if (country is null)
            {
                Console.WriteLine();
                Console.WriteLine("Country status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Country activated successfully."
                        : "Country deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing country status.");
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
        Console.WriteLine("=== Countries ===");
        Console.WriteLine();
    }

    private static void PrintCountries(IReadOnlyList<CountryListItemViewModel> countries)
    {
        if (countries.Count == 0)
        {
            Console.WriteLine("No countries found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Name",-35} " +
            $"{"Code",-10} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 62));

        foreach (var country in countries)
        {
            Console.WriteLine(
                $"{country.CountryId,4}  " +
                $"{country.Name,-35} " +
                $"{country.Code,-10} " +
                $"{(country.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(CountryDetailsViewModel country)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Country ID: {country.CountryId}");
        Console.WriteLine($"Name:       {country.Name}");
        Console.WriteLine($"Code:       {country.Code}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:     {(country.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(UpdateCountryViewModel country)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Country ID: {country.CountryId}");
        Console.WriteLine($"Name:       {country.Name}");
        Console.WriteLine($"Code:       {country.Code}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:     {(country.IsActive ? "Yes" : "No")}");
    }

    private static void PrintStatusChange(CountryDetailsViewModel country)
    {
        Console.WriteLine("--- Country ---");
        Console.WriteLine($"Country ID: {country.CountryId}");
        Console.WriteLine($"Name:       {country.Name}");
        Console.WriteLine($"Code:       {country.Code}");

        Console.WriteLine();

        Console.WriteLine("--- Current Status ---");
        Console.WriteLine($"Active:     {(country.IsActive ? "Yes" : "No")}");
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