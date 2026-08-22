namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu()
    {
        Items =
        [
            new MenuItem("1", "Dashboard"),
            new MenuItem("2", "Coins"),
            new MenuItem("3", "Collections"),
            new MenuItem("4", "Data"),
            new MenuItem("5", "Administration"),
        ];
    }
}