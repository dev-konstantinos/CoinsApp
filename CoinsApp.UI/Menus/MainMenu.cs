namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    private readonly AdminMenu _administrationMenu;
    private readonly CoinMenu _coinMenu;
    private readonly CollectionMenu _collectionMenu;

    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu(
        AdminMenu administrationMenu,
        CoinMenu coinMenu,
        CollectionMenu collectionMenu)
    {
        _administrationMenu =
            administrationMenu
            ?? throw new ArgumentNullException(nameof(administrationMenu));

        _coinMenu =
            coinMenu
            ?? throw new ArgumentNullException(nameof(coinMenu));

        _collectionMenu =
            collectionMenu
            ?? throw new ArgumentNullException(nameof(collectionMenu));

        Items =
        [
            new MenuItem("1", "Dashboard"),
            new MenuItem("2", "Coins", _coinMenu.RunAsync),
            new MenuItem("3", "Collections", _collectionMenu.RunAsync),
            new MenuItem("4", "Data"),
            new MenuItem(
                "5",
                "Administration",
                _administrationMenu.RunAsync),
        ];
    }
}