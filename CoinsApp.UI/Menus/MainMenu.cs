namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    private readonly AdminMenu _administrationMenu;

    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu(AdminMenu administrationMenu)
    {
        _administrationMenu = administrationMenu ?? throw new ArgumentNullException(nameof(administrationMenu));

        Items =
        [
            new MenuItem("1", "Dashboard"),
            new MenuItem("2", "Coins"),
            new MenuItem("3", "Collections"),
            new MenuItem("4", "Data"),
            new MenuItem("5", "Administration", _administrationMenu.Run),
        ];
    }
}