using CoinsApp.BLL.Coins;
using CoinsApp.BLL.Coins.ViewModels;

namespace CoinsApp.UI.Menus;

internal sealed class CoinMenu
{
    private readonly CoinService _coinService;

    public CoinMenu(CoinService coinService)
    {
        _coinService = coinService ?? throw new ArgumentNullException(nameof(coinService));
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Coins ===");
            Console.WriteLine();
            Console.WriteLine("1. List Coins");
            Console.WriteLine("2. Details");
            Console.WriteLine("3. Create");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ListCoins();
                    break;

                case "2":
                    ShowDetails();
                    break;

                case "3":
                    CreateCoin();
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

    private void ListCoins()
    {
        Console.Clear();

        Console.WriteLine("=== Coins ===");
        Console.WriteLine();

        try
        {
            var coins = _coinService.GetAll();

            if (coins.Count == 0)
            {
                Console.WriteLine("No coins found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintHeader();

            foreach (var coin in coins)
            {
                PrintCoin(coin);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coins.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private void ShowDetails()
    {
        Console.Clear();

        Console.WriteLine("=== Coin Details ===");
        Console.WriteLine();

        Console.Write("Coin ID: ");

        if (!int.TryParse(Console.ReadLine(), out var coinId))
        {
            Console.WriteLine();
            Console.WriteLine("Invalid Coin ID.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }

        try
        {
            var coin = _coinService.GetById(coinId);

            if (coin is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Coin with ID {coinId} was not found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintDetails(coin);

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coin.");
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private static void PrintHeader()
    {
        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Country",-28} " +
            $"{"Currency",-8} " +
            $"{"Denomination",-22} " +
            $"{"Year",6} " +
            $"{"Mint",-18} " +
            $"{"Current Price",16}");

        Console.WriteLine(new string('-', 110));
    }

    private static void PrintCoin(CoinListItemViewModel coin)
    {
        var price = coin.CurrentPrice.HasValue ? $"{coin.CurrentPrice.Value:0.####} {coin.CurrentPriceCurrency ?? ""}".Trim() : "-";

        Console.WriteLine(
            $"{coin.CoinId,4}  " +
            $"{coin.Country,-28} " +
            $"{coin.Currency,-8} " +
            $"{coin.Denomination,-22} " +
            $"{(coin.Year?.ToString() ?? "-"),6} " +
            $"{(coin.Mint ?? "-"),-18} " +
            $"{price,16}");
    }

    private static void PrintDetails(CoinDetailsViewModel coin)
    {
        Console.Clear();

        Console.WriteLine("=== Coin Details ===");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Coin ID:        {coin.CoinId}");
        Console.WriteLine($"Collection ID:  {coin.CollectionId}");
        Console.WriteLine($"Country:        {coin.CountryName}");
        Console.WriteLine($"Currency:       {coin.CurrencyCode} - {coin.CurrencyName}");
        Console.WriteLine($"Denomination:   {coin.DenominationDisplayName}");
        Console.WriteLine($"Year:           {coin.Year?.ToString() ?? "-"}");
        Console.WriteLine($"Mint:           {coin.MintName ?? "-"}");
        Console.WriteLine($"Mint Mark:      {coin.MintMark ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Physical ---");
        Console.WriteLine($"Material:       {coin.MaterialName ?? "-"}");
        Console.WriteLine($"Fineness:       {FormatDecimal(coin.Fineness)}");
        Console.WriteLine($"Weight:         {FormatDecimal(coin.Weight)}");
        Console.WriteLine($"Diameter:       {FormatDecimal(coin.Diameter)}");
        Console.WriteLine($"Thickness:      {FormatDecimal(coin.Thickness)}");
        Console.WriteLine($"Shape:          {coin.Shape ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Description ---");
        Console.WriteLine($"Designer:       {coin.Designer ?? "-"}");
        Console.WriteLine($"Mintage:        {coin.Mintage?.ToString() ?? "-"}");
        Console.WriteLine($"Description:    {coin.Description ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Condition ---");
        Console.WriteLine($"Condition:      {coin.Condition ?? "-"}");
        Console.WriteLine($"Grade:          {coin.Grade ?? "-"}");
        Console.WriteLine($"Grading Company:{coin.GradingCompany ?? "-"}");
        Console.WriteLine(
            $"Certificate:    {coin.GradingCertificateNumber ?? "-"}");
        Console.WriteLine();

        Console.WriteLine("--- Current Price ---");

        var price = coin.CurrentPrice.HasValue
            ? $"{coin.CurrentPrice.Value:0.####} {coin.CurrentPriceCurrencyCode ?? string.Empty}".Trim()
            : "-";

        Console.WriteLine($"Price:          {price}");
        Console.WriteLine(
            $"Price Date:     {coin.CurrentPriceDate?.ToString("yyyy-MM-dd HH:mm") ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Notes ---");
        Console.WriteLine(coin.Notes ?? "-");
    }

    private static string FormatDecimal(decimal? value)
    {
        return value?.ToString("0.####") ?? "-";
    }

    private void CreateCoin()
    {
        Console.Clear();

        Console.WriteLine("=== Create Coin ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCoinViewModel
            {
                CollectionId = ReadRequired<int>("Collection ID"),
                CountryId = ReadRequired<int>("Country ID"),
                CurrencyId = ReadRequired<int>("Currency ID"),
                DenominationId = ReadRequired<int>("Denomination ID"),

                MintId = ReadNullable<int>("Mint ID"),
                MaterialId = ReadNullable<int>("Material ID"),

                Year = ReadNullable<short>("Year"),
                MintMark = ReadNullableString("Mint Mark"),

                Fineness = ReadNullable<decimal>("Fineness"),
                Weight = ReadNullable<decimal>("Weight"),
                Diameter = ReadNullable<decimal>("Diameter"),
                Thickness = ReadNullable<decimal>("Thickness"),

                Shape = ReadNullableString("Shape"),
                Description = ReadNullableString("Description"),
                Designer = ReadNullableString("Designer"),
                Mintage = ReadNullable<long>("Mintage"),

                Condition = ReadNullableString("Condition"),
                Grade = ReadNullableString("Grade"),
                GradingCompany = ReadNullableString("Grading Company"),
                GradingCertificateNumber = ReadNullableString("Grading Certificate Number"),

                CurrentPrice = ReadNullable<decimal>("Current Price"),
                CurrentPriceCurrencyId = ReadNullable<int>("Current Price Currency ID"),

                CurrentPriceDate = ReadNullable<DateTime>("Current Price Date"),

                Notes = ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating coin...");

            var coinId = _coinService.Create(model);

            Console.WriteLine();
            Console.WriteLine($"Coin created successfully. Coin ID: {coinId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating coin.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private static T ReadRequired<T>(string label) where T : IParsable<T>
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine();

            if (T.TryParse(input, null, out var value))
            {
                return value;
            }

            Console.WriteLine($"Please enter a valid {typeof(T).Name}.");
        }
    }

    private static T? ReadNullable<T>(string label) where T : struct, IParsable<T>
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (T.TryParse(input, null, out var value))
            {
                return value;
            }

            Console.WriteLine($"Please enter a valid {typeof(T).Name}.");
        }
    }

    private static string? ReadNullableString(string label)
    {
        Console.Write($"{label} (empty = null): ");

        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }
}