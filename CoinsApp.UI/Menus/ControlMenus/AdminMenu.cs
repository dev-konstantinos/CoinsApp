using CoinsApp.BLL.Technical.Install;
using CoinsApp.BLL.Technical.Reset;
using CoinsApp.BLL.Technical.Status;

namespace CoinsApp.UI.Menus.ControlMenus;

internal sealed class AdminMenu
{
    private readonly InstallService _installService;
    private readonly ResetService _resetService;
    private readonly DatabaseStatusService _databaseStatusService;

    public AdminMenu(
        InstallService installService,
        ResetService resetService,
        DatabaseStatusService databaseStatusService)
    {
        _installService =
            installService
            ?? throw new ArgumentNullException(nameof(installService));

        _resetService =
            resetService
            ?? throw new ArgumentNullException(nameof(resetService));

        _databaseStatusService =
            databaseStatusService
            ?? throw new ArgumentNullException(nameof(databaseStatusService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Administration ===");
            Console.WriteLine();
            Console.WriteLine("1. Install Database");
            Console.WriteLine("2. Reset Database");
            Console.WriteLine("3. Database Status");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await InstallAsync();
                    break;

                case "2":
                    await ResetAsync();
                    break;

                case "3":
                    await ShowStatusAsync();
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

    private Task InstallAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Install ===");
        Console.WriteLine();

        var result = _installService.Install();

        Console.WriteLine(result.Message);
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();

        return Task.CompletedTask;
    }

    private async Task ResetAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Reset ===");
        Console.WriteLine();

        var result = await _resetService.ResetAsync();

        Console.WriteLine(result.Message);
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task ShowStatusAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Database Status ===");
        Console.WriteLine();

        var result = await _databaseStatusService.GetStatusAsync();

        Console.WriteLine(
            $"Status:          {(result.Success ? "Available" : "Not available")}");

        if (result.Success)
        {
            Console.WriteLine($"Server:          {result.ServerName}");
            Console.WriteLine($"Database:        {result.DatabaseName}");
            Console.WriteLine($"SQL Version:     {result.SqlServerVersion}");
            Console.WriteLine($"Edition:         {result.SqlServerEdition}");
            Console.WriteLine($"Product Level:   {result.SqlServerLevel}");
        }
        else
        {
            Console.WriteLine($"Details:         {result.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}