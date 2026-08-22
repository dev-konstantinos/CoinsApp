using CoinsApp.BLL.Database;
using CoinsApp.DAL.Configuration;
using CoinsApp.DAL.Database;
using CoinsApp.UI.Menus;

namespace CoinsApp.UI;

internal static class Program
{
    private static void Main()
    {
        var connectionString = "Server=localhost;Database=CoinsApp;Trusted_Connection=True;TrustServerCertificate=True;";

        var databaseOptions = new DatabaseOptions(connectionString);

        IDatabaseStatusService databaseStatusService = new DatabaseStatusProvider(databaseOptions);

        var mainMenu = new MainMenu();
        var menuRunner = new MenuRunner(mainMenu);

        menuRunner.Run();
    }
}