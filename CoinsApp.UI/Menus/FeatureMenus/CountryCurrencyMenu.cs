using CoinsApp.BLL.Features.CountryCurrencies;
using CoinsApp.BLL.Features.CountryCurrencies.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CountryCurrencyMenu
{
    private readonly ICountryCurrencyService _countryCurrencyService;

    public CountryCurrencyMenu(
        ICountryCurrencyService countryCurrencyService)
    {
        _countryCurrencyService =
            countryCurrencyService
            ?? throw new ArgumentNullException(
                nameof(countryCurrencyService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Country Currencies ===");
            Console.WriteLine();
            Console.WriteLine("1. List country-currency relationships");
            Console.WriteLine("2. Relationship detailed information");
            Console.WriteLine("3. List relationships by country");
            Console.WriteLine("4. List relationships by currency");
            Console.WriteLine("5. Create relationship");
            Console.WriteLine("6. Activate / Deactivate relationship");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input =
                Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await ListAsync();
                    break;

                case "2":
                    await DetailsAsync();
                    break;

                case "3":
                    await ListByCountryAsync();
                    break;

                case "4":
                    await ListByCurrencyAsync();
                    break;

                case "5":
                    await CreateAsync();
                    break;

                case "6":
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

        Console.WriteLine("=== Country-Currency Relationships ===");
        Console.WriteLine();

        try
        {
            var relationships =
                await _countryCurrencyService.GetAllAsync();

            PrintList(relationships);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading country-currency relationships.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Country-Currency Relationship Details ===");
        Console.WriteLine();

        var relationshipId =
            MenuInput.ReadRequiredId("CountryCurrency ID");

        try
        {
            var relationship =
                await _countryCurrencyService.GetByIdAsync(
                    relationshipId);

            if (relationship is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Country-currency relationship with ID " +
                    $"{relationshipId} was not found.");
            }
            else
            {
                Console.WriteLine();
                PrintDetails(relationship);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading country-currency relationship.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task ListByCountryAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Country-Currency Relationships by Country ===");
        Console.WriteLine();

        var countryId =
            MenuInput.ReadRequiredId("Country ID");

        try
        {
            var relationships =
                await _countryCurrencyService.GetByCountryAsync(
                    countryId);

            PrintList(relationships);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading relationships for country.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task ListByCurrencyAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Country-Currency Relationships by Currency ===");
        Console.WriteLine();

        var currencyId =
            MenuInput.ReadRequiredId("Currency ID");

        try
        {
            var relationships =
                await _countryCurrencyService.GetByCurrencyAsync(
                    currencyId);

            PrintList(relationships);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading relationships for currency.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Country-Currency Relationship ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCountryCurrencyViewModel
            {
                CountryId =
                    MenuInput.ReadRequiredId("Country ID"),

                CurrencyId =
                    MenuInput.ReadRequiredId("Currency ID"),

                IsActive =
                    MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine(
                "Creating country-currency relationship...");

            var relationship =
                await _countryCurrencyService.CreateAsync(model);

            if (relationship is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Country-currency relationship could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Country-currency relationship created successfully.");

                Console.WriteLine(
                    $"CountryCurrency ID: " +
                    $"{relationship.CountryCurrencyId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error creating country-currency relationship.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task SetActiveAsync()
    {
        Console.Clear();

        Console.WriteLine(
            "=== Activate / Deactivate Country-Currency Relationship ===");
        Console.WriteLine();

        var relationshipId =
            MenuInput.ReadRequiredId("CountryCurrency ID");

        try
        {
            var current =
                await _countryCurrencyService.GetByIdAsync(
                    relationshipId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Country-currency relationship with ID " +
                    $"{relationshipId} was not found.");

                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Country: {current.CountryName} " +
                $"({current.CountryCode})");

            Console.WriteLine(
                $"Currency: {current.CurrencyName} " +
                $"({current.CurrencyCode})");

            Console.WriteLine(
                $"Current status: " +
                $"{(current.IsActive ? "Active" : "Inactive")}");

            Console.WriteLine();

            var newStatus =
                !current.IsActive;

            Console.WriteLine(
                $"New status: " +
                $"{(newStatus ? "Active" : "Inactive")}");

            Console.WriteLine();

            var confirmation =
                MenuInput.ReadRequiredString(
                    $"Type {(newStatus ? "ACTIVATE" : "DEACTIVATE")} to confirm");

            if (!string.Equals(
                    confirmation,
                    newStatus
                        ? "ACTIVATE"
                        : "DEACTIVATE",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Action cancelled.");

                Pause();
                return;
            }

            var model = new SetCountryCurrencyActiveViewModel
            {
                CountryCurrencyId =
                    current.CountryCurrencyId,

                IsActive =
                    newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating relationship..."
                    : "Deactivating relationship...");

            var relationship =
                await _countryCurrencyService.SetActiveAsync(model);

            if (relationship is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Relationship status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Relationship activated successfully."
                        : "Relationship deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error changing relationship status.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private static void PrintList(
        IReadOnlyList<CountryCurrencyListItemViewModel>
            relationships)
    {
        if (relationships.Count == 0)
        {
            Console.WriteLine(
                "No country-currency relationships found.");

            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Country",-30} " +
            $"{"Code",-8} " +
            $"{"Currency",-25} " +
            $"{"Code",-8} " +
            $"{"Symbol",-8} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 99));

        foreach (var relationship in relationships)
        {
            Console.WriteLine(
                $"{relationship.CountryCurrencyId,4}  " +
                $"{relationship.CountryName,-30} " +
                $"{relationship.CountryCode,-8} " +
                $"{relationship.CurrencyName,-25} " +
                $"{relationship.CurrencyCode,-8} " +
                $"{relationship.CurrencySymbol ?? "",-8} " +
                $"{(relationship.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(
        CountryCurrencyDetailsViewModel relationship)
    {
        Console.WriteLine(
            $"CountryCurrency ID: " +
            $"{relationship.CountryCurrencyId}");

        Console.WriteLine(
            $"Country ID: {relationship.CountryId}");

        Console.WriteLine(
            $"Country: {relationship.CountryName}");

        Console.WriteLine(
            $"Country Code: {relationship.CountryCode}");

        Console.WriteLine(
            $"Currency ID: {relationship.CurrencyId}");

        Console.WriteLine(
            $"Currency: {relationship.CurrencyName}");

        Console.WriteLine(
            $"Currency Code: {relationship.CurrencyCode}");

        Console.WriteLine(
            $"Currency Symbol: " +
            $"{relationship.CurrencySymbol ?? ""}");

        Console.WriteLine(
            $"Active: " +
            $"{(relationship.IsActive ? "Yes" : "No")}");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}