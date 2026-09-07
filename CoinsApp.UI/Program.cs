using CoinsApp.BLL.Sale;
using CoinsApp.DAL.Sale;
using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Install;
using CoinsApp.BLL.PriceHistory;
using CoinsApp.BLL.Purchase;
using CoinsApp.BLL.Reset;
using CoinsApp.BLL.Status;
using CoinsApp.DAL.Coins;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.PriceHistory;
using CoinsApp.DAL.Purchase;
using CoinsApp.UI.Menus;
using Microsoft.Extensions.Configuration;

namespace CoinsApp.UI;

internal static class Program
{
    private static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: false)
            .Build();

        var connectionString =
            configuration.GetConnectionString("CoinsApp")
            ?? throw new InvalidOperationException(
                "Connection string 'CoinsApp' was not found.");

        // -------------------------------------------------
        // Administration
        // -------------------------------------------------

        var installService =
            new InstallService(connectionString);

        var resetService =
            new ResetService(connectionString);

        var databaseStatusService =
            new DatabaseStatusService(connectionString);

        var adminMenu =
            new AdminMenu(
                installService,
                resetService,
                databaseStatusService);

        // -------------------------------------------------
        // Database
        // -------------------------------------------------

        var databaseConnection =
            new DatabaseConnection(connectionString);

        // -------------------------------------------------
        // Coins
        // -------------------------------------------------

        var coinRepository =
            new CoinRepository(databaseConnection);

        var coinService =
            new CoinService(coinRepository);

        // -------------------------------------------------
        // Price History
        // -------------------------------------------------

        var priceHistoryRepository =
            new PriceHistoryRepository(databaseConnection);

        var priceHistoryService =
            new PriceHistoryService(priceHistoryRepository);

        var priceHistoryMenu =
            new PriceHistoryMenu(priceHistoryService);

    // -------------------------------------------------
    // Purchases
    // -------------------------------------------------

        var purchaseRepository =
            new PurchaseRepository(databaseConnection);

        var purchaseService =
            new PurchaseService(purchaseRepository);

        var purchaseMenu =
            new PurchaseMenu(purchaseService);

        // -------------------------------------------------
        // Sales
        // -------------------------------------------------

        var saleRepository =
            new SaleRepository(databaseConnection);

        var saleService =
            new SaleService(saleRepository);

        var saleMenu =
            new SaleMenu(saleService);

        // -------------------------------------------------
        // Coin Menu
        // -------------------------------------------------

        var coinMenu =
            new CoinMenu(
                coinService,
                priceHistoryMenu,
                purchaseMenu,
                saleMenu);

        // -------------------------------------------------
        // Main Menu
        // -------------------------------------------------

        var mainMenu =
            new MainMenu(
                adminMenu,
                coinMenu);

        var menuRunner =
            new MenuRunner(mainMenu);

        await menuRunner.RunAsync();
    }
}