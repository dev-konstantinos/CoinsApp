using CoinsApp.BLL.Purchase;
using CoinsApp.BLL.Purchase.ViewModels;

namespace CoinsApp.UI.Menus;

internal sealed class PurchaseMenu
{
    private readonly PurchaseService _purchaseService;

public PurchaseMenu(PurchaseService purchaseService)
    {
        _purchaseService =
            purchaseService
            ?? throw new ArgumentNullException(nameof(purchaseService));
    }

    public async Task RunAsync(int coinId)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Coin Purchases ===");
            Console.WriteLine($"Coin ID: {coinId}");
            Console.WriteLine();
            Console.WriteLine("1. List purchases");
            Console.WriteLine("2. Purchase details");
            Console.WriteLine("3. Create purchase");
            Console.WriteLine("4. Update purchase");
            Console.WriteLine("5. Delete purchase");
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
                    await DetailsAsync(coinId);
                    break;

                case "3":
                    await CreateAsync(coinId);
                    break;

                case "4":
                    await UpdateAsync(coinId);
                    break;

                case "5":
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

        Console.WriteLine("=== Purchases ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        try
        {
            var purchases =
                await _purchaseService.GetByCoinAsync(coinId);

            if (purchases.Count == 0)
            {
                Console.WriteLine("No purchases found.");
            }
            else
            {
                Console.WriteLine(
                    $"{"ID",4}  " +
                    $"{"Date",-19} " +
                    $"{"Price",16} " +
                    $"{"Seller",-25}");

                Console.WriteLine(new string('-', 75));

                foreach (var purchase in purchases)
                {
                    var seller =
                        purchase.SellerName ?? "-";

                    var price =
                        $"{purchase.PurchasePrice:0.####} " +
                        purchase.CurrencyCode;

                    Console.WriteLine(
                        $"{purchase.PurchaseId,4}  " +
                        $"{purchase.PurchaseDate:yyyy-MM-dd HH:mm:ss} " +
                        $"{price,16} " +
                        $"{seller,-25}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading purchases.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DetailsAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Purchase Details ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        var purchaseId =
            ReadRequiredId("Purchase ID");

        try
        {
            var purchase =
                await _purchaseService.GetByIdAsync(purchaseId);

            if (purchase is null ||
                purchase.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Purchase with ID {purchaseId} " +
                    $"was not found for this coin.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Purchase Details ===");
            Console.WriteLine();

            PrintDetails(purchase);

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading purchase.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task CreateAsync(int coinId)
    {
        Console.Clear();

        Console.WriteLine("=== Create Purchase ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        try
        {
            var model = new CreatePurchaseViewModel
            {
                CoinId = coinId,
                PurchaseDate =
                    ReadRequiredDateTime("Purchase Date"),

                PurchasePrice =
                    ReadRequiredPrice("Purchase Price"),

                CurrencyId =
                    ReadRequiredId("Currency ID"),

                SellerId =
                    ReadNullableId("Seller ID"),

                Notes =
                    ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating purchase...");

            var purchaseId =
                await _purchaseService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Purchase created successfully. " +
                $"Purchase ID: {purchaseId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating purchase.");
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

        Console.WriteLine("=== Update Purchase ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        var purchaseId =
            ReadRequiredId("Purchase ID");

        try
        {
            var current =
                await _purchaseService.GetByIdAsync(purchaseId);

            if (current is null ||
                current.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Purchase with ID {purchaseId} " +
                    $"was not found for this coin.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Purchase ===");
            Console.WriteLine($"Coin ID: {coinId}");
            Console.WriteLine($"Purchase ID: {purchaseId}");
            Console.WriteLine();

            var model = new UpdatePurchaseViewModel
            {
                PurchaseId = purchaseId,

                PurchaseDate =
                    ReadKeepCurrentDateTime(
                        "Purchase Date",
                        current.PurchaseDate),

                PurchasePrice =
                    ReadKeepCurrentPrice(
                        "Purchase Price",
                        current.PurchasePrice),

                CurrencyId =
                    ReadKeepCurrentId(
                        "Currency ID",
                        current.CurrencyId),

                SellerId =
                    ReadKeepCurrentNullableId(
                        "Seller ID",
                        current.SellerId),

                Notes =
                    ReadKeepCurrentString(
                        "Notes",
                        current.Notes)
            };

            Console.WriteLine();
            Console.WriteLine("Updating purchase...");

            await _purchaseService.UpdateAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                "Purchase updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating purchase.");
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

        Console.WriteLine("=== Delete Purchase ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();

        var purchaseId =
            ReadRequiredId("Purchase ID");

        try
        {
            var current =
                await _purchaseService.GetByIdAsync(purchaseId);

            if (current is null ||
                current.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Purchase with ID {purchaseId} " +
                    $"was not found for this coin.");

                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Delete Purchase ===");
            Console.WriteLine();

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine(
                "Delete this purchase? (y/n)");
            Console.Write("Confirm: ");

            var confirmation =
                Console.ReadLine()?.Trim()
                    .ToLowerInvariant();

            if (confirmation != "y")
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            var model = new DeletePurchaseViewModel
            {
                PurchaseId = purchaseId
            };

            await _purchaseService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                "Purchase deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting purchase.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private static void PrintDetails(
        PurchaseDetailsViewModel purchase)
    {
        Console.WriteLine(
            $"Purchase ID:   {purchase.PurchaseId}");

        Console.WriteLine(
            $"Coin ID:        {purchase.CoinId}");

        Console.WriteLine(
            $"Date:           " +
            $"{purchase.PurchaseDate:yyyy-MM-dd HH:mm:ss}");

        Console.WriteLine(
            $"Price:          " +
            $"{purchase.PurchasePrice:0.####} " +
            $"{purchase.CurrencyCode}");

        Console.WriteLine(
            $"Currency:       " +
            $"{purchase.CurrencyCode} - " +
            $"{purchase.CurrencyName}");

        Console.WriteLine(
            $"Seller ID:      " +
            $"{purchase.SellerId?.ToString() ?? "-"}");

        Console.WriteLine(
            $"Seller:         " +
            $"{purchase.SellerName ?? "-"}");

        Console.WriteLine(
            $"Notes:          " +
            $"{purchase.Notes ?? "-"}");
    }

    private static int ReadRequiredId(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input =
                Console.ReadLine()?.Trim();

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID.");
        }
    }

    private static int? ReadNullableId(string label)
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID " +
                "or leave empty.");
        }
    }

    private static int? ReadKeepCurrentNullableId(
        string label,
        int? currentValue)
    {
        while (true)
        {
            var currentText =
                currentValue?.ToString() ?? "null";

            Console.Write(
                $"{label} [{currentText}] " +
                "(Enter = keep, null = clear): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (input.Equals(
                    "null",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID, " +
                "or 'null' to clear.");
        }
    }

    private static decimal ReadRequiredPrice(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input =
                Console.ReadLine()?.Trim();

            if (decimal.TryParse(
                    input,
                    out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price " +
                "greater than or equal to zero.");
        }
    }

    private static decimal ReadKeepCurrentPrice(
        string label,
        decimal currentValue)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{currentValue:0.####}] " +
                "(Enter = keep current): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (decimal.TryParse(
                    input,
                    out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price " +
                "greater than or equal to zero.");
        }
    }

    private static DateTime ReadRequiredDateTime(
        string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input =
                Console.ReadLine()?.Trim();

            if (DateTime.TryParse(
                    input,
                    out var value))
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
                $"{label} " +
                $"[{currentValue:yyyy-MM-dd HH:mm:ss}] " +
                "(Enter = keep current): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (DateTime.TryParse(
                    input,
                    out var value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date and time.");
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

            var input =
                Console.ReadLine()?.Trim();

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
                "Please enter a valid positive ID " +
                "or press Enter to keep the current value.");
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
        var currentText =
            string.IsNullOrWhiteSpace(currentValue)
                ? "null"
                : currentValue;

        Console.Write(
            $"{label} [{currentText}] " +
            "(Enter = keep, null = clear): ");

        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (input.Trim().Equals(
                "null",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input.Trim();
    }
}
