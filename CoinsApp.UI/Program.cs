using CoinsApp.UI.Menus;

namespace CoinsApp.UI;

internal static class Program
{
    private static void Main()
    {
        var mainMenu = new MainMenu();
        var menuRunner = new MenuRunner(mainMenu);

        menuRunner.Run();
    }
}