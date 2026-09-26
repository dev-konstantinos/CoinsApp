namespace CoinsApp.UI.Menus;

internal sealed class MainMenu
{
    private readonly AdminMenu _administrationMenu;
    private readonly CoinMenu _coinMenu;
    private readonly CollectionMenu _collectionMenu;
    private readonly ContactMenu _contactMenu;
    private readonly CatalogMenu _catalogMenu;
    private readonly CatalogEntryMenu _catalogEntryMenu;
    private readonly CountryMenu _countryMenu;
    private readonly CurrencyMenu _currencyMenu;
    private readonly CountryCurrencyMenu _countryCurrencyMenu;
    private readonly MintMenu _mintMenu;
    private readonly DenominationMenu _denominationMenu;
    private readonly MaterialMenu _materialMenu;

    public IReadOnlyList<MenuItem> Items { get; }

    public MainMenu(
        AdminMenu administrationMenu,
        CoinMenu coinMenu,
        CollectionMenu collectionMenu,
        ContactMenu contactMenu,
        CatalogMenu catalogMenu,
        CatalogEntryMenu catalogEntryMenu,
        CountryMenu countryMenu,
        CurrencyMenu currencyMenu,
        CountryCurrencyMenu countryCurrencyMenu,
        MintMenu mintMenu,
        DenominationMenu denominationMenu,
        MaterialMenu materialMenu)
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

        _catalogMenu =
            catalogMenu
            ?? throw new ArgumentNullException(nameof(catalogMenu));

        _catalogEntryMenu =
            catalogEntryMenu
            ?? throw new ArgumentNullException(nameof(catalogEntryMenu));

        _countryMenu =
            countryMenu
            ?? throw new ArgumentNullException(nameof(countryMenu));
        
        _currencyMenu =
            currencyMenu
            ?? throw new ArgumentNullException(nameof(currencyMenu));

        _countryCurrencyMenu =
            countryCurrencyMenu
            ?? throw new ArgumentNullException(nameof(countryCurrencyMenu));

        _mintMenu =
            mintMenu
            ?? throw new ArgumentNullException(nameof(mintMenu));

        _denominationMenu =
            denominationMenu
            ?? throw new ArgumentNullException(nameof(denominationMenu));

        _materialMenu =
            materialMenu
            ?? throw new ArgumentNullException(nameof(materialMenu));

        Items =
        [
            new MenuItem("A", "Administration", _administrationMenu.RunAsync),
            new MenuItem("B", "Coins", _coinMenu.RunAsync),
            new MenuItem("C", "Collections", _collectionMenu.RunAsync),
            new MenuItem("D", "Contacts", _contactMenu.RunAsync),
            new MenuItem("E", "Catalogs", _catalogMenu.RunAsync),
            new MenuItem("F", "Catalog Entries", _catalogEntryMenu.RunAsync),
            new MenuItem("G", "Countries", _countryMenu.RunAsync),
            new MenuItem("H", "Currencies", _currencyMenu.RunAsync),
            new MenuItem("I", "Country Currencies", _countryCurrencyMenu.RunAsync),
            new MenuItem("J", "Mints", _mintMenu.RunAsync),
            new MenuItem("K", "Denominations", _denominationMenu.RunAsync),
            new MenuItem("L", "Materials", _materialMenu.RunAsync)
        ];
    }
}
