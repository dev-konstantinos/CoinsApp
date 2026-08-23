using CoinsApp.BLL.Install;
using CoinsApp.UI.Menus;
using Microsoft.Extensions.Configuration;

namespace CoinsApp.UI;

internal static class Program
{
    private static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var connectionString =
            configuration.GetConnectionString("CoinsApp")
            ?? throw new InvalidOperationException("Connection string 'CoinsApp' was not found.");

        var installService =
            new InstallService(connectionString);

        var adminMenu =
            new AdminMenu(installService);

        var mainMenu =
            new MainMenu(adminMenu);

        var menuRunner =
            new MenuRunner(mainMenu);

        menuRunner.Run();
    }
}