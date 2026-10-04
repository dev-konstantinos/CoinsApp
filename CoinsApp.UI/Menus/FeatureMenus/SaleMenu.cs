using CoinsApp.BLL.Features.Sales;
using CoinsApp.BLL.Features.Sales.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class SaleMenu
{
    private readonly ISaleService _saleService;

    public SaleMenu(ISaleService saleService)
    {
        _saleService =
            saleService
            ?? throw new ArgumentNullException(nameof(saleService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync(int coinId)
    {
        while (true)
        {
            Console.Clear();

            PrintHeader(coinId);

            Console.WriteLine("1. List sales");
            Console.WriteLine("2. Sale details");
            Console.WriteLine("3. Create sale");
            Console.WriteLine("4. Update sale");
            Console.WriteLine("5. Delete sale");
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
                    await DetailsAsync(coinId);
                    break;

                case 3:
                    await CreateAsync(coinId);
                    break;

                case 4:
                    await UpdateAsync(coinId);
                    break;

                case 5:
                    await DeleteAsync(coinId);
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Pause();
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
            var sales = await _saleService.GetByCoinAsync(coinId);

            PrintSales(sales);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading sales.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Sale Details");
        Console.WriteLine();

        var saleId = MenuInput.ReadIdOrExit("Sale ID");

        if (saleId is null)
        {
            return;
        }

        try
        {
            var sale = await _saleService.GetByIdAsync(saleId.Value);

            if (sale is null || sale.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Sale with ID {saleId.Value} " +
                    $"was not found for this coin.");
            }
            else
            {
                PrintDetails(sale);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading sale.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Create Sale");
        Console.WriteLine();

        try
        {
            var model = new CreateSaleViewModel
            {
                CoinId = coinId,
                SaleDate = MenuInput.ReadRequiredDateTime("Sale Date"),
                SalePrice = MenuInput.ReadRequiredPrice("Sale Price"),
                CurrencyId = MenuInput.ReadRequiredId("Currency ID"),
                BuyerId = MenuInput.ReadRequiredId("Buyer ID"),
                Notes = MenuInput.ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating sale...");

            var saleId = await _saleService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine("Sale created successfully.");
            Console.WriteLine($"Sale ID: {saleId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating sale.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Update Sale");
        Console.WriteLine();

        var saleId = MenuInput.ReadIdOrExit("Sale ID");

        if (saleId is null)
        {
            return;
        }

        try
        {
            var current = await _saleService.GetByIdAsync(saleId.Value);

            if (current is null || current.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Sale with ID {saleId.Value} " +
                    $"was not found for this coin.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader(coinId);
            Console.WriteLine("Update Sale");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine("Type null to clear optional values.");
            Console.WriteLine();

            var model = new UpdateSaleViewModel
            {
                SaleId = current.SaleId,
                SaleDate = MenuInput.ReadKeepCurrentDateTime(
                    "Sale Date",
                    current.SaleDate),
                SalePrice = MenuInput.ReadKeepCurrentPrice(
                    "Sale Price",
                    current.SalePrice),
                CurrencyId = MenuInput.ReadKeepCurrentId(
                    "Currency ID",
                    current.CurrencyId),
                BuyerId = MenuInput.ReadKeepCurrentId(
                    "Buyer ID",
                    current.BuyerId),
                Notes = MenuInput.ReadKeepCurrentString(
                    "Notes",
                    current.Notes)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating sale...");

            await _saleService.UpdateAsync(model);

            Console.WriteLine();
            Console.WriteLine("Sale updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating sale.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync(int coinId)
    {
        Console.Clear();

        PrintHeader(coinId);
        Console.WriteLine("Delete Sale");
        Console.WriteLine();

        var saleId = MenuInput.ReadIdOrExit("Sale ID");

        if (saleId is null)
        {
            return;
        }

        try
        {
            var current = await _saleService.GetByIdAsync(saleId.Value);

            if (current is null || current.CoinId != coinId)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Sale with ID {saleId.Value} " +
                    $"was not found for this coin.");

                Pause();
                return;
            }

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("WARNING: This sale will be deleted.");
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

            var model = new DeleteSaleViewModel
            {
                SaleId = current.SaleId
            };

            Console.WriteLine();
            Console.WriteLine("Deleting sale...");

            await _saleService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine("Sale deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting sale.");
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
        Console.WriteLine("=== Coin Sales ===");
        Console.WriteLine($"Coin ID: {coinId}");
        Console.WriteLine();
    }

    private static void PrintSales(
        IReadOnlyList<SaleListItemViewModel> sales)
    {
        if (sales.Count == 0)
        {
            Console.WriteLine("No sales found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Date",-19} " +
            $"{"Price",16} " +
            $"{"Buyer",-25}");

        Console.WriteLine(new string('-', 75));

        foreach (var sale in sales)
        {
            var buyer = sale.BuyerName ?? "-";

            var price =
                $"{sale.SalePrice:0.####} " +
                sale.CurrencyCode;

            Console.WriteLine(
                $"{sale.SaleId,4}  " +
                $"{sale.SaleDate:yyyy-MM-dd HH:mm:ss} " +
                $"{price,16} " +
                $"{buyer,-25}");
        }
    }

    private static void PrintDetails(
        SaleDetailsViewModel sale)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Sale ID:   {sale.SaleId}");
        Console.WriteLine($"Coin ID:   {sale.CoinId}");

        Console.WriteLine();

        Console.WriteLine("--- Sale ---");
        Console.WriteLine(
            $"Date:      {sale.SaleDate:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine(
            $"Price:     {sale.SalePrice:0.####} {sale.CurrencyCode}");

        Console.WriteLine();

        Console.WriteLine("--- Currency ---");
        Console.WriteLine($"Currency ID: {sale.CurrencyId}");
        Console.WriteLine($"Currency:    {sale.CurrencyCode}");
        Console.WriteLine($"Name:        {sale.CurrencyName}");

        Console.WriteLine();

        Console.WriteLine("--- Buyer ---");
        Console.WriteLine($"Buyer ID:   {sale.BuyerId}");
        Console.WriteLine($"Buyer:      {sale.BuyerName ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Additional Information ---");
        Console.WriteLine($"Notes:      {sale.Notes ?? "-"}");
    }

    private static void PrintUpdateSummary(
        UpdateSaleViewModel sale)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Sale ID:   {sale.SaleId}");

        Console.WriteLine();

        Console.WriteLine("--- Sale ---");
        Console.WriteLine(
            $"Date:      {sale.SaleDate:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine(
            $"Price:     {sale.SalePrice:0.####}");

        Console.WriteLine();

        Console.WriteLine("--- Currency ---");
        Console.WriteLine($"Currency ID: {sale.CurrencyId}");

        Console.WriteLine();

        Console.WriteLine("--- Buyer ---");
        Console.WriteLine($"Buyer ID:   {sale.BuyerId}");

        Console.WriteLine();

        Console.WriteLine("--- Additional Information ---");
        Console.WriteLine($"Notes:      {sale.Notes ?? "-"}");
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