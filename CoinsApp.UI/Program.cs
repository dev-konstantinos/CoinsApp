using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Install;
using CoinsApp.BLL.Reset;
using CoinsApp.BLL.Status;
using CoinsApp.DAL.Coins;
using CoinsApp.DAL.Database;
using CoinsApp.UI.Menus;
using Microsoft.Extensions.Configuration;

namespace CoinsApp.UI;

internal static class Program
{
    private static void Main()
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
        // Coins
        // -------------------------------------------------

        var databaseConnection =
            new DatabaseConnection(connectionString);

        var coinRepository =
            new CoinRepository(databaseConnection);

        var coinService =
            new CoinService(coinRepository);

        var coinMenu =
            new CoinMenu(coinService);

        // -------------------------------------------------
        // Main Menu
        // -------------------------------------------------

        var mainMenu =
            new MainMenu(
                adminMenu,
                coinMenu);

        var menuRunner =
            new MenuRunner(mainMenu);

        menuRunner.Run();
    }
}