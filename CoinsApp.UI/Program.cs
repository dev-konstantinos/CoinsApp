using CoinsApp.BLL.CatalogEntries;
using CoinsApp.BLL.Catalogs;
using CoinsApp.BLL.CoinImages;
using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Collection;
using CoinsApp.BLL.Contacts;
using CoinsApp.BLL.Countries;
using CoinsApp.BLL.CountryCurrencies;
using CoinsApp.BLL.Currencies;
using CoinsApp.BLL.Denominations;
using CoinsApp.BLL.Install;
using CoinsApp.BLL.Materials;
using CoinsApp.BLL.Mints;
using CoinsApp.BLL.PriceHistory;
using CoinsApp.BLL.Purchase;
using CoinsApp.BLL.Reset;
using CoinsApp.BLL.Sale;
using CoinsApp.BLL.Status;
using CoinsApp.BLL.Users;
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
using CoinsApp.DAL.Denominations;
using CoinsApp.DAL.Materials;
using CoinsApp.DAL.Mints;
using CoinsApp.DAL.PriceHistory;
using CoinsApp.DAL.Purchase;
using CoinsApp.DAL.Sale;
using CoinsApp.DAL.Users;
using CoinsApp.UI.Menus;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CoinsApp.UI;

internal static class Program
{
    private static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

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

        var services = new ServiceCollection();

        services.AddSingleton(new DatabaseConnection(connectionString));

        // -------------------------------------------------
        // Administration
        // -------------------------------------------------

        services.AddScoped<InstallService>();
        services.AddScoped<ResetService>();
        services.AddScoped<DatabaseStatusService>();
        services.AddScoped<AdminMenu>();

        // -------------------------------------------------
        // Coins
        // -------------------------------------------------

        services.AddScoped<ICoinRepository, CoinRepository>();
        services.AddScoped<ICoinService, CoinService>();

        // -------------------------------------------------
        // Price History
        // -------------------------------------------------

        services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
        services.AddScoped<IPriceHistoryService, PriceHistoryService>();

        // -------------------------------------------------
        // Purchases
        // -------------------------------------------------

        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IPurchaseService, PurchaseService>();

        // -------------------------------------------------
        // Sales
        // -------------------------------------------------

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISaleService, SaleService>();

        // -------------------------------------------------
        // Collections
        // -------------------------------------------------

        services.AddScoped<ICollectionRepository, CollectionRepository>();
        services.AddScoped<ICollectionService, CollectionService>();

        // -------------------------------------------------
        // Contacts
        // -------------------------------------------------

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();

        // -------------------------------------------------
        // Catalogs
        // -------------------------------------------------

        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICatalogService, CatalogService>();

        // -------------------------------------------------
        // Catalog Entries
        // -------------------------------------------------

        services.AddScoped<ICatalogEntryRepository, CatalogEntryRepository>();
        services.AddScoped<ICatalogEntryService, CatalogEntryService>();

        // -------------------------------------------------
        // Coin Images
        // -------------------------------------------------

        services.AddScoped<ICoinImageRepository, CoinImageRepository>();
        services.AddScoped<ICoinImageService, CoinImageService>();

        // -------------------------------------------------
        // Countries
        // -------------------------------------------------

        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountryService, CountryService>();

        // -------------------------------------------------
        // Currencies
        // -------------------------------------------------

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ICurrencyService, CurrencyService>();

        // -------------------------------------------------
        // Denominations
        // -------------------------------------------------

        services.AddScoped<IDenominationRepository, DenominationRepository>();
        services.AddScoped<IDenominationService, DenominationService>();

        // -------------------------------------------------
        // CountryCurrency
        // -------------------------------------------------

        services.AddScoped<ICountryCurrencyRepository, CountryCurrencyRepository>();
        services.AddScoped<ICountryCurrencyService, CountryCurrencyService>();

        // -------------------------------------------------
        // Mints
        // -------------------------------------------------

        services.AddScoped<IMintRepository, MintRepository>();
        services.AddScoped<IMintService, MintService>();

        // -------------------------------------------------
        // Materials
        // -------------------------------------------------

        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IMaterialService, MaterialService>();

        // -------------------------------------------------
        // Users
        // -------------------------------------------------

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<PasswordHasher>();
        services.AddScoped<IUserService, UserService>();

        // -------------------------------------------------
        // UI Menus
        // -------------------------------------------------

        services.AddScoped<PriceHistoryMenu>();
        services.AddScoped<PurchaseMenu>();
        services.AddScoped<SaleMenu>();
        services.AddScoped<CollectionMenu>();
        services.AddScoped<ContactMenu>();
        services.AddScoped<CatalogMenu>();
        services.AddScoped<CatalogEntryMenu>();
        services.AddScoped<CoinMenu>();
        services.AddScoped<CoinImageMenu>();
        services.AddScoped<CountryMenu>();
        services.AddScoped<CurrencyMenu>();
        services.AddScoped<DenominationMenu>();
        services.AddScoped<CountryCurrencyMenu>();
        services.AddScoped<MintMenu>();
        services.AddScoped<MaterialMenu>();
        services.AddScoped<UserMenu>();

        // -------------------------------------------------
        // Main Menu
        // -------------------------------------------------

        services.AddScoped<MainMenu>();
        services.AddScoped<MenuRunner>();

        // -------------------------------------------------
        // Build the service provider and run the application
        // -------------------------------------------------

        using var serviceProvider = services.BuildServiceProvider();

        var menuRunner = serviceProvider.GetRequiredService<MenuRunner>();

        await menuRunner.RunAsync();
    }
}
