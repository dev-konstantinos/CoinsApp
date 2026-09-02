using CoinsApp.BLL.PriceHistory;
using CoinsApp.BLL.PriceHistory.ViewModels;

namespace CoinsApp.UI.Menus;

internal sealed class PriceHistoryMenu
{
    private readonly PriceHistoryService _priceHistoryService;

    public PriceHistoryMenu(
        PriceHistoryService priceHistoryService)
    {
        _priceHistoryService =
            priceHistoryService
            ?? throw new ArgumentNullException(nameof(priceHistoryService));
    }

    public async Task RunAsync(int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coinId));
        }

        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Price History ===");
            Console.WriteLine($"Coin ID: {coinId}");
            Console.WriteLine();
            Console.WriteLine("1. List");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Update");
            Console.WriteLine("4. Delete");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await ListAsync(coinId);
                    break;

                case "2":
                    await CreateAsync(coinId);
                    break;

                case "3":
                    await UpdateAsync(coinId);
                    break;

                case "4":
                    await DeleteAsync(coinId);
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

    private async Task ListAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Price History ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        try
        {
            var entries =
                await _priceHistoryService.GetByCoinAsync(coinId);

            if (entries.Count == 0)
            {
                Console.WriteLine("No price history found.");
            }
            else
            {
                foreach (var entry in entries)
                {
                    Console.WriteLine(
                        $"ID: {entry.PriceHistoryId}");

                    Console.WriteLine(
                        $"Price: {entry.Price:0.0000} {entry.CurrencyCode}");

                    Console.WriteLine(
                        $"Date: {entry.PriceDate:yyyy-MM-dd HH:mm:ss}");

                    Console.WriteLine(
                        $"Currency: {entry.CurrencyName}");

                    if (!string.IsNullOrWhiteSpace(entry.Source))
                    {
                        Console.WriteLine(
                            $"Source: {entry.Source}");
                    }

                    if (!string.IsNullOrWhiteSpace(entry.Notes))
                    {
                        Console.WriteLine(
                            $"Notes: {entry.Notes}");
                    }

                    Console.WriteLine(
                        "--------------------------------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task CreateAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Create Price History ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        try
        {
            var model = new CreatePriceHistoryViewModel
            {
                CoinId = coinId,
                Price = ReadRequiredPrice("Price"),
                CurrencyId = ReadRequiredId("Currency ID"),
                PriceDate = ReadRequiredDateTime("Price Date"),
                Source = ReadNullableString("Source"),
                Notes = ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating price history...");

            var priceHistoryId =
                await _priceHistoryService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Price history created successfully. ID: {priceHistoryId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task UpdateAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Update Price History ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        var priceHistoryId =
            ReadRequiredId("Price History ID");

        try
        {
            var entries =
                await _priceHistoryService.GetByCoinAsync(coinId);

            var current =
                entries.FirstOrDefault(
                    x => x.PriceHistoryId == priceHistoryId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Price history entry with ID {priceHistoryId} " +
                    $"was not found for this coin.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Price History ===");
            Console.WriteLine($"Coin ID: {coinId}");
            Console.WriteLine($"Price History ID: {priceHistoryId}");
            Console.WriteLine();

            var model = new UpdatePriceHistoryViewModel
            {
                PriceHistoryId = priceHistoryId,

                Price = ReadKeepCurrentPrice(
                    "Price",
                    current.Price),

                CurrencyId = ReadKeepCurrentId(
                    "Currency ID",
                    current.CurrencyId),

                PriceDate = ReadKeepCurrentDateTime(
                    "Price Date",
                    current.PriceDate),

                Source = ReadKeepCurrentString(
                    "Source",
                    current.Source),

                Notes = ReadKeepCurrentString(
                    "Notes",
                    current.Notes)
            };

            Console.WriteLine();
            Console.WriteLine("Updating price history...");

            await _priceHistoryService.UpdateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                "Price history updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DeleteAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Delete Price History ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        var priceHistoryId =
            ReadRequiredId("Price History ID");

        try
        {
            var entries =
                await _priceHistoryService.GetByCoinAsync(coinId);

            var current =
                entries.FirstOrDefault(
                    x => x.PriceHistoryId == priceHistoryId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Price history entry with ID {priceHistoryId} " +
                    $"was not found for this coin.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Delete Price History ===");
            Console.WriteLine();

            Console.WriteLine(
                $"Price History ID: {current.PriceHistoryId}");

            Console.WriteLine(
                $"Price: {current.Price:0.0000} {current.CurrencyCode}");

            Console.WriteLine(
                $"Date: {current.PriceDate:yyyy-MM-dd HH:mm:ss}");

            Console.WriteLine(
                $"Currency: {current.CurrencyName}");

            if (!string.IsNullOrWhiteSpace(current.Source))
            {
                Console.WriteLine(
                    $"Source: {current.Source}");
            }

            if (!string.IsNullOrWhiteSpace(current.Notes))
            {
                Console.WriteLine(
                    $"Notes: {current.Notes}");
            }

            Console.WriteLine();
            Console.WriteLine(
                "Delete this price history entry? (y/n)");
            Console.Write("Confirm: ");

            var confirmation =
                Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirmation != "y")
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            var model = new DeletePriceHistoryViewModel
            {
                PriceHistoryId = priceHistoryId
            };

            await _priceHistoryService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                "Price history deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting price history.");
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

            var input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID.");
        }
    }

    private static int ReadKeepCurrentId(
        string label,
        int currentValue)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{currentValue}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID.");
        }
    }

    private static decimal ReadRequiredPrice(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine()?.Trim();

            if (decimal.TryParse(input, out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price greater than or equal to zero.");
        }
    }

    private static decimal ReadKeepCurrentPrice(
        string label,
        decimal currentValue)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{currentValue:0.0000}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (decimal.TryParse(input, out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price greater than or equal to zero.");
        }
    }

    private static DateTime ReadRequiredDateTime(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine()?.Trim();

            if (DateTime.TryParse(input, out var value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date and time.");
        }
    }

    private static DateTime ReadKeepCurrentDateTime(
        string label,
        DateTime currentValue)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{currentValue:yyyy-MM-dd HH:mm:ss}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (DateTime.TryParse(input, out var value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date and time.");
        }
    }

    private static string? ReadNullableString(
        string label)
    {
        Console.Write($"{label} (optional): ");

        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input)
            ? null
            : input.Trim();
    }

    private static string? ReadKeepCurrentString(
        string label,
        string? currentValue)
    {
        var displayValue =
            string.IsNullOrWhiteSpace(currentValue)
                ? "empty"
                : currentValue;

        Console.Write(
            $"{label} [{displayValue}] " +
            "(Enter = keep current): ");

        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input)
            ? currentValue
            : input.Trim();
    }
}