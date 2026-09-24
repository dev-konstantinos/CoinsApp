using CoinsApp.BLL.CatalogEntries;
using CoinsApp.BLL.Catalogs;
using CoinsApp.BLL.CoinImages;
using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Collection;
using CoinsApp.BLL.Contacts;
using CoinsApp.BLL.Countries;
using CoinsApp.BLL.CountryCurrencies;
using CoinsApp.BLL.Currencies;
using CoinsApp.BLL.Install;
using CoinsApp.BLL.PriceHistory;
using CoinsApp.BLL.Purchase;
using CoinsApp.BLL.Reset;
using CoinsApp.BLL.Sale;
using CoinsApp.BLL.Status;
using CoinsApp.DAL.CatalogEntries;
using CoinsApp.DAL.Catalogs;
using CoinsApp.DAL.CoinImages;
using CoinsApp.DAL.Coins;
using CoinsApp.DAL.Collection;
using CoinsApp.DAL.Contacts;
using CoinsApp.DAL.Countries;
using CoinsApp.DAL.CountryCurrencies;
using CoinsApp.DAL.Currencies;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.PriceHistory;
using CoinsApp.DAL.Purchase;
using CoinsApp.DAL.Sale;
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
        // Collections
        // -------------------------------------------------

        var collectionRepository =
            new CollectionRepository(databaseConnection);

        var collectionService =
            new CollectionService(collectionRepository);

        var collectionMenu =
            new CollectionMenu(collectionService);

        // -------------------------------------------------
        // Contacts
        // -------------------------------------------------

        var contactRepository =
            new ContactRepository(databaseConnection);

        var contactService =
            new ContactService(contactRepository);

        var contactMenu =
            new ContactMenu(contactService);

        // -------------------------------------------------
        // Catalogs
        // -------------------------------------------------

        var catalogRepository =
            new CatalogRepository(databaseConnection);

        var catalogService =
            new CatalogService(catalogRepository);

        var catalogMenu =
            new CatalogMenu(catalogService);

        // -------------------------------------------------
        // Catalog Entries
        // -------------------------------------------------

        var catalogEntryRepository =
            new CatalogEntryRepository(databaseConnection);

        var catalogEntryService =
            new CatalogEntryService(catalogEntryRepository);

        var catalogEntryMenu =
            new CatalogEntryMenu(catalogEntryService);

        // -------------------------------------------------
        // Coin Images
        // -------------------------------------------------

        var coinImageRepository =
            new CoinImageRepository(databaseConnection);

        var coinImageService =
            new CoinImageService(coinImageRepository);

        var coinImageMenu =
            new CoinImageMenu(coinImageService);

        // -------------------------------------------------
        // Countries
        // -------------------------------------------------

        var countryRepository =
            new CountryRepository(databaseConnection);

        var countryService =
            new CountryService(countryRepository);

        var countryMenu =
            new CountryMenu(countryService);

        // -------------------------------------------------
        // Currencies
        // -------------------------------------------------

        var currencyRepository =
            new CurrencyRepository(databaseConnection);

        var currencyService =
            new CurrencyService(currencyRepository);

        var currencyMenu =
            new CurrencyMenu(currencyService);

        // -------------------------------------------------
        // CountryCurrency
        // -------------------------------------------------

        var countryCurrencyRepository =
            new CountryCurrencyRepository(databaseConnection);

        var countryCurrencyService =
            new CountryCurrencyService(countryCurrencyRepository);

        var countryCurrencyMenu =
            new CountryCurrencyMenu(countryCurrencyService);

        // -------------------------------------------------
        // Coin Menu
        // -------------------------------------------------

        var coinMenu =
            new CoinMenu(
                coinService,
                priceHistoryMenu,
                purchaseMenu,
                saleMenu,
                coinImageMenu);

        // -------------------------------------------------
        // Main Menu
        // -------------------------------------------------

        var mainMenu =
            new MainMenu(
                adminMenu,
                coinMenu,
                collectionMenu,
                contactMenu,
                catalogMenu,
                catalogEntryMenu,
                countryMenu,
                currencyMenu,
                countryCurrencyMenu);

        var menuRunner =
            new MenuRunner(mainMenu);

        await menuRunner.RunAsync();
    }
}