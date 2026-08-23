using CoinsApp.BLL.Install;

namespace CoinsApp.UI.Menus;

internal sealed class AdminMenu
{
    private readonly InstallService _installService;

    public AdminMenu(InstallService installService)
    {
        _installService = installService ?? throw new ArgumentNullException(nameof(installService));
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Administration ===");
            Console.WriteLine();
            Console.WriteLine("1. Install Database");
            Console.WriteLine("2. Reset Database");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Install();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void Install()
    {
        Console.Clear();

        Console.WriteLine("=== Install ===");
        Console.WriteLine();

        var result = _installService.Install();

        Console.WriteLine(result.Message);
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}