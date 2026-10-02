using CoinsApp.BLL.Features.CatalogEntries;
using CoinsApp.BLL.Features.Catalogs;
using CoinsApp.BLL.Features.CoinImages;
using CoinsApp.BLL.Features.Coins;
using CoinsApp.BLL.Features.Collections;
using CoinsApp.BLL.Features.Contacts;
using CoinsApp.BLL.Features.Countries;
using CoinsApp.BLL.Features.CountryCurrencies;
using CoinsApp.BLL.Features.Currencies;
using CoinsApp.BLL.Features.Denominations;
using CoinsApp.BLL.Features.Materials;
using CoinsApp.BLL.Features.Mints;
using CoinsApp.BLL.Features.PriceHistories;
using CoinsApp.BLL.Features.Purchases;
using CoinsApp.BLL.Features.Sales;
using CoinsApp.BLL.Features.Users;
using CoinsApp.BLL.Technical.Install;
using CoinsApp.BLL.Technical.Reset;
using CoinsApp.BLL.Technical.Status;
using CoinsApp.DAL.Technical.Database;
using CoinsApp.DAL.Features.CatalogEntries;
using CoinsApp.DAL.Features.Catalogs;
using CoinsApp.DAL.Features.CoinImages;
using CoinsApp.DAL.Features.Coins;
using CoinsApp.DAL.Features.Collections;
using CoinsApp.DAL.Features.Contacts;
using CoinsApp.DAL.Features.Countries;
using CoinsApp.DAL.Features.CountryCurrencies;
using CoinsApp.DAL.Features.Currencies;
using CoinsApp.DAL.Features.Denominations;
using CoinsApp.DAL.Features.Materials;
using CoinsApp.DAL.Features.Mints;
using CoinsApp.DAL.Features.PriceHistories;
using CoinsApp.DAL.Features.Purchases;
using CoinsApp.DAL.Features.Sales;
using CoinsApp.DAL.Features.Users;
using CoinsApp.UI.Menus.FeatureMenus;
using CoinsApp.UI.Technical;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using CoinsApp.UI.Menus;

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

        services.AddScoped(_ => new InstallService(connectionString));

        services.AddScoped(_ => new ResetService(connectionString));

        services.AddScoped(_ => new DatabaseStatusService(connectionString));

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
