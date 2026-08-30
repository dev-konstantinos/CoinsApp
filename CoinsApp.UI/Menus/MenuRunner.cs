namespace CoinsApp.UI.Menus;

internal sealed class MenuRunner
{
    private readonly MainMenu _menu;

    public MenuRunner(MainMenu menu)
    {
        _menu =
            menu
            ?? throw new ArgumentNullException(nameof(menu));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            DisplayMenu();

            var input = Console.ReadLine()?.Trim();

            if (string.Equals(
                input,
                "0",
                StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var selectedItem = _menu.Items
                .FirstOrDefault(item =>
                    string.Equals(
                        item.Key,
                        input,
                        StringComparison.OrdinalIgnoreCase));

            if (selectedItem is null)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                continue;
            }

            await ExecuteAsync(selectedItem);
        }
    }

    private void DisplayMenu()
    {
        Console.Clear();

        Console.WriteLine("=== CoinsApp ===");
        Console.WriteLine();

        foreach (var item in _menu.Items)
        {
            Console.WriteLine($"{item.Key}. {item.Title}");
        }

        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Select: ");
    }

    private static async Task ExecuteAsync(MenuItem item)
    {
        Console.WriteLine();
        Console.WriteLine($"Selected: {item.Title}");
        Console.WriteLine();

        await item.ExecuteAsync();
    }
}