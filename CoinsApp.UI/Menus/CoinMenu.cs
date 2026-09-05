using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Coins.ViewModels;
using CoinsApp.BLL.PriceHistory;

namespace CoinsApp.UI.Menus;

internal sealed class CoinMenu
{
    private readonly CoinService _coinService;
    private readonly PriceHistoryMenu _priceHistoryMenu;
    private readonly PurchaseMenu _purchaseMenu;

    public CoinMenu(
        CoinService coinService,
        PriceHistoryMenu priceHistoryMenu,
        PurchaseMenu purchaseMenu)
    {
        _coinService =
            coinService
            ?? throw new ArgumentNullException(nameof(coinService));

        _priceHistoryMenu =
            priceHistoryMenu
            ?? throw new ArgumentNullException(nameof(priceHistoryMenu));

        _purchaseMenu =
            purchaseMenu
            ?? throw new ArgumentNullException(nameof(purchaseMenu));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Coins ===");
            Console.WriteLine();
            Console.WriteLine("1. List of coins");
            Console.WriteLine("2. Coin details");
            Console.WriteLine("3. Create a coin");
            Console.WriteLine("4. Update a coin");
            Console.WriteLine("5. Delete a coin");
            Console.WriteLine("6. Price history");
            Console.WriteLine("7. Purchases");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await ListCoinsAsync();
                    break;

                case "2":
                    await ShowDetailsAsync();
                    break;

                case "3":
                    await CreateCoinAsync();
                    break;

                case "4":
                    await UpdateCoinAsync();
                    break;

                case "5":
                    await DeleteCoinAsync();
                    break;

                case "6":
                    await PriceHistoryAsync();
                    break;

                case "7":
                    await PurchasesAsync();
                    break;

                case "0":
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

    private async Task ListCoinsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Coins ===");
        Console.WriteLine();

        try
        {
            var coins = await _coinService.GetAllAsync();

            if (coins.Count == 0)
            {
                Console.WriteLine("No coins found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintHeader();

            foreach (var coin in coins)
            {
                PrintCoin(coin);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coins.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task ShowDetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Coin Details ===");
        Console.WriteLine();

        var coinId = ReadRequiredId("Coin ID");

        try
        {
            var coin = await _coinService.GetByIdAsync(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin with ID {coinId} was not found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintDetails(coin);

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coin.");
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
    
    private async Task CreateCoinAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Coin ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCoinViewModel
            {
                CollectionId = ReadRequiredId("Collection ID"),
                CountryId = ReadRequiredId("Country ID"),
                CurrencyId = ReadRequiredId("Currency ID"),
                DenominationId = ReadRequiredId("Denomination ID"),

                MintId = ReadNullableId("Mint ID"),
                MaterialId = ReadNullableId("Material ID"),

                Year = ReadNullable<short>("Year"),
                MintMark = ReadNullableString("Mint Mark"),

                Fineness = ReadNullable<decimal>("Fineness"),
                Weight = ReadNullable<decimal>("Weight"),
                Diameter = ReadNullable<decimal>("Diameter"),
                Thickness = ReadNullable<decimal>("Thickness"),

                Shape = ReadNullableString("Shape"),
                Description = ReadNullableString("Description"),
                Designer = ReadNullableString("Designer"),
                Mintage = ReadNullable<long>("Mintage"),

                Condition = ReadNullableString("Condition"),
                Grade = ReadNullableString("Grade"),
                GradingCompany = ReadNullableString("Grading Company"),
                GradingCertificateNumber =
                    ReadNullableString("Grading Certificate Number"),

                CurrentPrice =
                    ReadNullable<decimal>("Current Price"),

                CurrentPriceCurrencyId =
                    ReadNullableId("Current Price Currency ID"),

                CurrentPriceDate =
                    ReadNullable<DateTime>("Current Price Date"),

                Notes = ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating coin...");

            var coinId = await _coinService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Coin created successfully. Coin ID: {coinId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating coin.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DeleteCoinAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Coin ===");
        Console.WriteLine();

        var coinId = ReadRequiredId("Coin ID");

        try
        {
            var coin = await _coinService.GetByIdAsync(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin with ID {coinId} was not found.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Delete Coin ===");
            Console.WriteLine();

            PrintDetails(coin);

            Console.WriteLine();
            Console.WriteLine("=== WARNING ===");
            Console.WriteLine();

            Console.WriteLine(
                "Deleting this coin will also delete:");

            Console.WriteLine("- catalog entries");
            Console.WriteLine("- images");
            Console.WriteLine("- price history");
            Console.WriteLine("- purchases");
            Console.WriteLine("- sales");

            Console.WriteLine();

            Console.Write(
                "Type DELETE to confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(
                    confirmation,
                    "DELETE",
                    StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            var model = new DeleteCoinViewModel
            {
                CoinId = coin.CoinId
            };

            Console.WriteLine();
            Console.WriteLine("Deleting coin...");

            var deletedCoinId =
                await _coinService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Coin deleted successfully. " +
                $"Coin ID: {deletedCoinId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting coin.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private static int ReadRequiredId(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine();

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid ID greater than zero.");
        }
    }

    private async Task UpdateCoinAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Coin ===");
        Console.WriteLine();

        var coinId = ReadRequiredId("Coin ID");

        try
        {
            var coin = await _coinService.GetByIdAsync(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin with ID {coinId} was not found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Coin ===");
            Console.WriteLine();

            Console.WriteLine("--- Current Coin ---");
            PrintDetails(coin);

            Console.WriteLine();
            Console.WriteLine("--- Enter New Values ---");
            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCoinViewModel
            {
                CoinId = coin.CoinId,

                CollectionId =
                    ReadKeepCurrentId(
                        "Collection ID",
                        coin.CollectionId),

                CountryId =
                    ReadKeepCurrentId(
                        "Country ID",
                        coin.CountryId),

                CurrencyId =
                    ReadKeepCurrentId(
                        "Currency ID",
                        coin.CurrencyId),

                DenominationId =
                    ReadKeepCurrentId(
                        "Denomination ID",
                        coin.DenominationId),

                MintId =
                    ReadKeepCurrentNullableId(
                        "Mint ID",
                        coin.MintId),

                MaterialId =
                    ReadKeepCurrentNullableId(
                        "Material ID",
                        coin.MaterialId),

                Year =
                    ReadKeepCurrentNullable(
                        "Year",
                        coin.Year),

                MintMark =
                    ReadKeepCurrentString(
                        "Mint Mark",
                        coin.MintMark),

                Fineness =
                    ReadKeepCurrentNullable(
                        "Fineness",
                        coin.Fineness),

                Weight =
                    ReadKeepCurrentNullable(
                        "Weight",
                        coin.Weight),

                Diameter =
                    ReadKeepCurrentNullable(
                        "Diameter",
                        coin.Diameter),

                Thickness =
                    ReadKeepCurrentNullable(
                        "Thickness",
                        coin.Thickness),

                Shape =
                    ReadKeepCurrentString(
                        "Shape",
                        coin.Shape),

                Description =
                    ReadKeepCurrentString(
                        "Description",
                        coin.Description),

                Designer =
                    ReadKeepCurrentString(
                        "Designer",
                        coin.Designer),

                Mintage =
                    ReadKeepCurrentNullable(
                        "Mintage",
                        coin.Mintage),

                Condition =
                    ReadKeepCurrentString(
                        "Condition",
                        coin.Condition),

                Grade =
                    ReadKeepCurrentString(
                        "Grade",
                        coin.Grade),

                GradingCompany =
                    ReadKeepCurrentString(
                        "Grading Company",
                        coin.GradingCompany),

                GradingCertificateNumber =
                    ReadKeepCurrentString(
                        "Grading Certificate Number",
                        coin.GradingCertificateNumber),

                CurrentPrice =
                    ReadKeepCurrentNullable(
                        "Current Price",
                        coin.CurrentPrice),

                CurrentPriceCurrencyId =
                    ReadKeepCurrentNullableId(
                        "Current Price Currency ID",
                        coin.CurrentPriceCurrencyId),

                CurrentPriceDate =
                    ReadKeepCurrentNullable(
                        "Current Price Date",
                        coin.CurrentPriceDate),

                Notes =
                    ReadKeepCurrentString(
                        "Notes",
                        coin.Notes)
            };

            Console.WriteLine();
            Console.WriteLine("=== Update Preview ===");
            Console.WriteLine();

            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.Write("Save changes? (y/n): ");

            var confirmation =
                Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirmation != "y" &&
                confirmation != "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Update cancelled.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Updating coin...");

            var updatedCoinId =
                await _coinService.UpdateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Coin updated successfully. Coin ID: {updatedCoinId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating coin.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private static int? ReadKeepCurrentNullableId(
    string label,
    int? current)
    {
        while (true)
        {
            var currentText =
                current?.ToString() ?? "null";

            Console.Write(
                $"{label} [{currentText}] " +
                "(Enter = keep, null = clear): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (input.Trim().Equals(
                    "null",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input.Trim(), out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid ID greater than zero, " +
                "or use 'null' to clear the value.");
        }
    }

    private static int? ReadNullableId(string label)
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input.Trim(), out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid ID greater than zero, " +
                "or leave empty for null.");
        }
    }

    private static int ReadKeepCurrentId(
    string label,
    int current)
    {
        while (true)
        {
            Console.Write($"{label} [{current}]: ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (int.TryParse(input.Trim(), out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid ID greater than zero, " +
                "or press Enter to keep the current value.");
        }
    }

    private static T? ReadKeepCurrentNullable<T>(
    string label,
    T? current)
    where T : struct, IParsable<T>
    {
        while (true)
        {
            var currentText =
                current?.ToString() ?? "null";

            Console.Write(
                $"{label} [{currentText}] " +
                "(Enter = keep, null = clear): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (input.Trim().Equals(
                    "null",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (T.TryParse(input.Trim(), null, out var value))
            {
                return value;
            }

            Console.WriteLine(
                $"Please enter a valid {typeof(T).Name}, " +
                "or use 'null' to clear the value.");
        }
    }

    private static string? ReadKeepCurrentString(
    string label,
    string? current)
    {
        var currentText = current ?? "null";

        Console.Write(
            $"{label} [{currentText}] " +
            "(Enter = keep, null = clear): ");

        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return current;
        }

        if (input.Trim().Equals(
                "null",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input.Trim();
    }

    private static void PrintUpdateSummary(
    UpdateCoinViewModel model)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine(
            $"Coin ID:        {model.CoinId}");
        Console.WriteLine(
            $"Collection ID:  {model.CollectionId}");
        Console.WriteLine(
            $"Country ID:     {model.CountryId}");
        Console.WriteLine(
            $"Currency ID:    {model.CurrencyId}");
        Console.WriteLine(
            $"Denomination ID:{model.DenominationId}");
        Console.WriteLine(
            $"Mint ID:        {model.MintId?.ToString() ?? "null"}");
        Console.WriteLine(
            $"Material ID:    {model.MaterialId?.ToString() ?? "null"}");
        Console.WriteLine(
            $"Year:           {model.Year?.ToString() ?? "null"}");
        Console.WriteLine(
            $"Mint Mark:      {model.MintMark ?? "null"}");
        Console.WriteLine();

        Console.WriteLine("--- Physical ---");
        Console.WriteLine(
            $"Fineness:       {FormatDecimal(model.Fineness)}");
        Console.WriteLine(
            $"Weight:         {FormatDecimal(model.Weight)}");
        Console.WriteLine(
            $"Diameter:       {FormatDecimal(model.Diameter)}");
        Console.WriteLine(
            $"Thickness:      {FormatDecimal(model.Thickness)}");
        Console.WriteLine(
            $"Shape:          {model.Shape ?? "null"}");
        Console.WriteLine();

        Console.WriteLine("--- Description ---");
        Console.WriteLine(
            $"Designer:       {model.Designer ?? "null"}");
        Console.WriteLine(
            $"Mintage:        {model.Mintage?.ToString() ?? "null"}");
        Console.WriteLine(
            $"Description:    {model.Description ?? "null"}");
        Console.WriteLine();

        Console.WriteLine("--- Condition ---");
        Console.WriteLine(
            $"Condition:      {model.Condition ?? "null"}");
        Console.WriteLine(
            $"Grade:          {model.Grade ?? "null"}");
        Console.WriteLine(
            $"Grading Company:{model.GradingCompany ?? "null"}");
        Console.WriteLine(
            $"Certificate:    " +
            $"{model.GradingCertificateNumber ?? "null"}");
        Console.WriteLine();

        Console.WriteLine("--- Current Price ---");

        var price =
            model.CurrentPrice.HasValue
                ? model.CurrentPrice.Value.ToString("0.####")
                : "null";

        Console.WriteLine(
            $"Price:          {price}");

        Console.WriteLine(
            $"Price Currency: " +
            $"{model.CurrentPriceCurrencyId?.ToString() ?? "null"}");

        Console.WriteLine(
            $"Price Date:     " +
            $"{model.CurrentPriceDate?.ToString("yyyy-MM-dd HH:mm") ?? "null"}");

        Console.WriteLine();

        Console.WriteLine("--- Notes ---");
        Console.WriteLine(
            model.Notes ?? "null");
    }

    private static void PrintHeader()
    {
        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Country",-28} " +
            $"{"Currency",-8} " +
            $"{"Denomination",-22} " +
            $"{"Year",6} " +
            $"{"Mint",-18} " +
            $"{"Current Price",16}");

        Console.WriteLine(new string('-', 110));
    }

    private static void PrintCoin(CoinListItemViewModel coin)
    {
        var price = coin.CurrentPrice.HasValue
            ? $"{coin.CurrentPrice.Value:0.####} " +
              $"{coin.CurrentPriceCurrency ?? ""}".Trim()
            : "-";

        Console.WriteLine(
            $"{coin.CoinId,4}  " +
            $"{coin.Country,-28} " +
            $"{coin.Currency,-8} " +
            $"{coin.Denomination,-22} " +
            $"{(coin.Year?.ToString() ?? "-"),6} " +
            $"{(coin.Mint ?? "-"),-18} " +
            $"{price,16}");
    }

    private static void PrintDetails(CoinDetailsViewModel coin)
    {
        Console.Clear();

        Console.WriteLine("=== Coin Details ===");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Coin ID:        {coin.CoinId}");
        Console.WriteLine($"Collection ID:  {coin.CollectionId}");
        Console.WriteLine($"Country:        {coin.CountryName}");
        Console.WriteLine(
            $"Currency:       {coin.CurrencyCode} - {coin.CurrencyName}");
        Console.WriteLine(
            $"Denomination:   {coin.DenominationDisplayName}");
        Console.WriteLine(
            $"Year:           {coin.Year?.ToString() ?? "-"}");
        Console.WriteLine($"Mint:           {coin.MintName ?? "-"}");
        Console.WriteLine($"Mint Mark:      {coin.MintMark ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Physical ---");
        Console.WriteLine(
            $"Material:       {coin.MaterialName ?? "-"}");
        Console.WriteLine(
            $"Fineness:       {FormatDecimal(coin.Fineness)}");
        Console.WriteLine(
            $"Weight:         {FormatDecimal(coin.Weight)}");
        Console.WriteLine(
            $"Diameter:       {FormatDecimal(coin.Diameter)}");
        Console.WriteLine(
            $"Thickness:      {FormatDecimal(coin.Thickness)}");
        Console.WriteLine($"Shape:          {coin.Shape ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Description ---");
        Console.WriteLine($"Designer:       {coin.Designer ?? "-"}");
        Console.WriteLine(
            $"Mintage:        {coin.Mintage?.ToString() ?? "-"}");
        Console.WriteLine(
            $"Description:    {coin.Description ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Condition ---");
        Console.WriteLine(
            $"Condition:      {coin.Condition ?? "-"}");
        Console.WriteLine($"Grade:          {coin.Grade ?? "-"}");
        Console.WriteLine(
            $"Grading Company:{coin.GradingCompany ?? "-"}");
        Console.WriteLine(
            $"Certificate:    {coin.GradingCertificateNumber ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Current Price ---");

        var price = coin.CurrentPrice.HasValue
            ? $"{coin.CurrentPrice.Value:0.####} " +
              $"{coin.CurrentPriceCurrencyCode ?? string.Empty}".Trim()
            : "-";

        Console.WriteLine($"Price:          {price}");
        Console.WriteLine(
            $"Price Date:     " +
            $"{coin.CurrentPriceDate?.ToString("yyyy-MM-dd HH:mm") ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Notes ---");
        Console.WriteLine(coin.Notes ?? "-");
    }

    private static string FormatDecimal(decimal? value)
    {
        return value?.ToString("0.####") ?? "-";
    }

    private static T? ReadNullable<T>(string label)
        where T : struct, IParsable<T>
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (T.TryParse(input, null, out var value))
            {
                return value;
            }

            Console.WriteLine(
                $"Please enter a valid {typeof(T).Name}.");
        }
    }

    private static string? ReadNullableString(string label)
    {
        Console.Write($"{label} (empty = null): ");

        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input)
            ? null
            : input.Trim();
    }

    private async Task PriceHistoryAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Coin Price History ===");
        Console.WriteLine();

        var coinId = ReadRequiredId("Coin ID");

        try
        {
            var coin = await _coinService.GetByIdAsync(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin with ID {coinId} was not found.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            await _priceHistoryMenu.RunAsync(coin.CoinId);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task PurchasesAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Coin Purchases ===");
        Console.WriteLine();

        var coinId = ReadRequiredId("Coin ID");

        try
        {
            var coin = await _coinService.GetByIdAsync(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin with ID {coinId} was not found.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            await _purchaseMenu.RunAsync(coin.CoinId);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading purchases.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}