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
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ListCoins();
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
}