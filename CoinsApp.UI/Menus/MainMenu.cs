namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    private readonly AdminMenu _administrationMenu;
    private readonly CoinMenu _coinMenu;

    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu(
        AdminMenu administrationMenu,
        CoinMenu coinMenu)
    {
        _administrationMenu =
            administrationMenu
            ?? throw new ArgumentNullException(nameof(administrationMenu));

        _coinMenu =
            coinMenu
            ?? throw new ArgumentNullException(nameof(coinMenu));

        Items =
        [
            new MenuItem("1", "Dashboard"),
            new MenuItem("2", "Coins", _coinMenu.Run),
            new MenuItem("3", "Collections"),
            new MenuItem("4", "Data"),
            new MenuItem("5", "Administration", _administrationMenu.Run),
        ];
    }
}