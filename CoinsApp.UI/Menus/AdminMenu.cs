using CoinsApp.BLL.Install;
using CoinsApp.BLL.Reset;

namespace CoinsApp.UI.Menus;

internal sealed class AdminMenu
{
    private readonly InstallService _installService;
    private readonly ResetService _resetService;

    public AdminMenu(InstallService installService, ResetService resetService)
    {
        _installService = installService ?? throw new ArgumentNullException(nameof(installService));

        _resetService = resetService ?? throw new ArgumentNullException(nameof(resetService));
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

                case "2":
                    Reset();
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

    private void Reset()
    {
        Console.Clear();

        Console.WriteLine("=== Reset ===");
        Console.WriteLine();

        var result = _resetService.Reset();

        Console.WriteLine(result.Message);
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}