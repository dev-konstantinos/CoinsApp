namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    private readonly AdminMenu _administrationMenu;
    private readonly CoinMenu _coinMenu;
    private readonly CollectionMenu _collectionMenu;
    private readonly ContactMenu _contactMenu;

    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu(
        AdminMenu administrationMenu,
        CoinMenu coinMenu,
        CollectionMenu collectionMenu,
        ContactMenu contactMenu)
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

        _contactMenu =
            contactMenu
            ?? throw new ArgumentNullException(nameof(contactMenu));

        Items =
        [
            new MenuItem("1", "Dashboard"),
            new MenuItem("2", "Coins", _coinMenu.RunAsync),
            new MenuItem("3", "Collections", _collectionMenu.RunAsync),
            new MenuItem("4", "Contacts", _contactMenu.RunAsync),
            new MenuItem("5", "Data"),
            
            new MenuItem(
                "6",
                "Administration",
                _administrationMenu.RunAsync),
        ];
    }
}