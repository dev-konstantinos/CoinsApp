using CoinsApp.BLL.Features.PriceHistories;
using CoinsApp.BLL.Features.PriceHistories.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class PriceHistoryMenu
{
    private readonly IPriceHistoryService _priceHistoryService;

    public PriceHistoryMenu(IPriceHistoryService priceHistoryService)
    {
        _priceHistoryService = priceHistoryService ?? throw new ArgumentNullException(nameof(priceHistoryService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync(int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coinId));
        }

        while (true)
        {
            Console.Clear();

            PrintHeader(coinId);

            Console.WriteLine("1. List price history");
            Console.WriteLine("2. Create price history");
            Console.WriteLine("3. Update price history");
            Console.WriteLine("4. Delete price history");
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
                    await ListAsync(coinId);
                    break;

                case 2:
                    await CreateAsync(coinId);
                    break;

                case 3:
                    await UpdateAsync(coinId);
                    break;

                case 4:
                    await DeleteAsync(coinId);
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

    private async Task ListAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);

        try
        {
            var entries = await _priceHistoryService.GetByCoinAsync(coinId);

            PrintPriceHistory(entries);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Create Price History");
        Console.WriteLine();

        try
        {
            var model = new CreatePriceHistoryViewModel
            {
                CoinId = coinId,
                Price = MenuInput.ReadRequiredPrice("Price"),
                CurrencyId = MenuInput.ReadRequiredId("Currency ID"),
                PriceDate = MenuInput.ReadRequiredDateTime("Price Date"),
                Source = MenuInput.ReadNullableString("Source"),
                Notes = MenuInput.ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating price history...");

            var priceHistoryId = await _priceHistoryService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine("Price history created successfully.");
            Console.WriteLine($"Price History ID: {priceHistoryId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Update Price History");
        Console.WriteLine();

        var priceHistoryId = MenuInput.ReadIdOrExit("Price History ID");

        if (priceHistoryId is null)
        {
            return;
        }

        try
        {
            var entries =
                await _priceHistoryService.GetByCoinAsync(coinId);

            var current = entries.FirstOrDefault(x => x.PriceHistoryId == priceHistoryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Price history entry with ID {priceHistoryId.Value} " +
                    $"was not found for this coin.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader(coinId);
            Console.WriteLine("Update Price History");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine("Type null to clear optional values.");
            Console.WriteLine();

            var model = new UpdatePriceHistoryViewModel
            {
                PriceHistoryId = current.PriceHistoryId,
                Price = MenuInput.ReadKeepCurrentPrice("Price", current.Price),
                CurrencyId = MenuInput.ReadKeepCurrentId("Currency ID", current.CurrencyId),
                PriceDate = MenuInput.ReadKeepCurrentDateTime("Price Date", current.PriceDate),
                Source = MenuInput.ReadKeepCurrentString("Source", current.Source),
                Notes = MenuInput.ReadKeepCurrentString("Notes", current.Notes)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating price history...");

            await _priceHistoryService.UpdateAsync(model);

            Console.WriteLine();
            Console.WriteLine("Price history updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Delete Price History");
        Console.WriteLine();

        var priceHistoryId = MenuInput.ReadIdOrExit("Price History ID");

        if (priceHistoryId is null)
        {
            return;
        }

        try
        {
            var entries = await _priceHistoryService.GetByCoinAsync(coinId);

            var current = entries.FirstOrDefault(x => x.PriceHistoryId == priceHistoryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Price history entry with ID {priceHistoryId.Value} " +
                    $"was not found for this coin.");

                Pause();
                return;
            }

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("WARNING: This price history entry will be deleted.");
            Console.WriteLine("Type DELETE to confirm.");
            Console.Write("Confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(
                    confirmation,
                    "DELETE",
                    StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Pause();
                return;
            }

            var model = new DeletePriceHistoryViewModel
            {
                PriceHistoryId = current.PriceHistoryId
            };

            Console.WriteLine();
            Console.WriteLine("Deleting price history...");

            await _priceHistoryService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine("Price history deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting price history.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader(int coinId)
    {
        Console.WriteLine("=== Price History ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();
    }

    private static void PrintPriceHistory(IReadOnlyList<PriceHistoryListItemViewModel> entries)
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No price history found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Price",14} " +
            $"{"Date",-20} " +
            $"{"Currency",-12} " +
            $"{"Source",-20}");

        Console.WriteLine(new string('-', 78));

        foreach (var entry in entries)
        {
            Console.WriteLine(
                $"{entry.PriceHistoryId,4}  " +
                $"{entry.Price,14:0.0000} " +
                $"{entry.PriceDate,-20:yyyy-MM-dd HH:mm:ss} " +
                $"{entry.CurrencyCode,-12} " +
                $"{entry.Source ?? "-",-20}");
        }
    }

    private static void PrintDetails(
        PriceHistoryListItemViewModel entry)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Price History ID: {entry.PriceHistoryId}");
        Console.WriteLine($"Coin ID:          {entry.CoinId}");

        Console.WriteLine();

        Console.WriteLine("--- Price ---");
        Console.WriteLine($"Price:            {entry.Price:0.0000}");
        Console.WriteLine($"Currency ID:      {entry.CurrencyId}");
        Console.WriteLine($"Currency:         {entry.CurrencyName}");
        Console.WriteLine($"Currency Code:    {entry.CurrencyCode}");
        Console.WriteLine($"Price Date:       {entry.PriceDate:yyyy-MM-dd HH:mm:ss}");

        Console.WriteLine();

        Console.WriteLine("--- Additional Information ---");
        Console.WriteLine($"Source:           {entry.Source ?? "-"}");
        Console.WriteLine($"Notes:            {entry.Notes ?? "-"}");
    }

    private static void PrintUpdateSummary(
        UpdatePriceHistoryViewModel entry)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Price History ID: {entry.PriceHistoryId}");

        Console.WriteLine();

        Console.WriteLine("--- Price ---");
        Console.WriteLine($"Price:            {entry.Price:0.0000}");
        Console.WriteLine($"Currency ID:      {entry.CurrencyId}");
        Console.WriteLine($"Price Date:       {entry.PriceDate:yyyy-MM-dd HH:mm:ss}");

        Console.WriteLine();

        Console.WriteLine("--- Additional Information ---");
        Console.WriteLine($"Source:           {entry.Source ?? "-"}");
        Console.WriteLine($"Notes:            {entry.Notes ?? "-"}");
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