using CoinsApp.BLL.Install;
using CoinsApp.DAL.Database;
using CoinsApp.DAL.Install;
using CoinsApp.UI.Menus;

namespace CoinsApp.UI;

internal static class Program
{
    private static void Main()
    {
        var connectionString =
            "YOUR_EXISTING_CONNECTION_STRING";

        var databaseConnection =
            new DatabaseConnection(connectionString);

        var databaseInstaller =
            new DatabaseInstaller(databaseConnection);

        var installService =
            new InstallService(databaseInstaller);

        var administrationMenu =
            new AdminMenu(installService);

        var mainMenu =
            new MainMenu(administrationMenu);

        var menuRunner =
            new MenuRunner(mainMenu);

        menuRunner.Run();
    }
}