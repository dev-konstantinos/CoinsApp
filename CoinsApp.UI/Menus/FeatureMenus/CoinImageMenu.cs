using CoinsApp.BLL.Features.CoinImages;
using CoinsApp.BLL.Features.CoinImages.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CoinImageMenu
{
    private readonly ICoinImageService _coinImageService;

    public CoinImageMenu(ICoinImageService coinImageService)
    {
        _coinImageService = coinImageService ?? throw new ArgumentNullException(nameof(coinImageService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            PrintHeader();

            Console.WriteLine("1. List coin images");
            Console.WriteLine("2. Image details");
            Console.WriteLine("3. List images by Coin");
            Console.WriteLine("4. Create image");
            Console.WriteLine("5. Update image");
            Console.WriteLine("6. Delete image");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    await ListAsync();
                    break;

                case 2:
                    await DetailsAsync();
                    break;

                case 3:
                    await ListByCoinAsync();
                    break;

                case 4:
                    await CreateAsync();
                    break;

                case 5:
                    await UpdateAsync();
                    break;

                case 6:
                    await DeleteAsync();
                    break;

                case 0:
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

    // ============================================================
    // CRUD
    // ============================================================

    private async Task ListAsync()
    {
        Console.Clear();

        PrintHeader();

        try
        {
            var images = await _coinImageService.GetAllAsync();

            Console.WriteLine();
            PrintImages(images);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coin images.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Details");
        Console.WriteLine();

        var coinImageId = MenuInput.ReadIdOrExit("Coin Image ID");

        if (coinImageId is null)
        {
            return;
        }

        try
        {
            var image = await _coinImageService.GetByIdAsync(coinImageId.Value);

            if (image is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Coin image with ID {coinImageId.Value} was not found.");
            }
            else
            {
                Console.WriteLine();
                PrintDetails(image);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coin image.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create");
        Console.WriteLine();

        try
        {
            var model = new CreateCoinImageViewModel
            {
                CoinId = MenuInput.ReadRequiredId("Coin ID"),
                ImageType = MenuInput.ReadRequiredString("Image type"),
                FileName = MenuInput.ReadRequiredString("File name"),
                FilePath = MenuInput.ReadRequiredString("File path"),
                Description = MenuInput.ReadNullableString("Description"),
                SortOrder = ReadCreateSortOrder()
            };

            Console.WriteLine();
            Console.WriteLine("Creating coin image...");

            var image = await _coinImageService.CreateAsync(model);

            if (image is null)
            {
                Console.WriteLine();
                Console.WriteLine("Coin image could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Coin image created successfully.");

                Console.WriteLine($"Coin Image ID: {image.CoinImageId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating coin image.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update");
        Console.WriteLine();

        var coinImageId = MenuInput.ReadIdOrExit("Coin Image ID");

        if (coinImageId is null)
        {
            return;
        }

        try
        {
            var current =
                await _coinImageService.GetByIdAsync(
                    coinImageId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Coin image with ID {coinImageId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCoinImageViewModel
            {
                CoinImageId = current.CoinImageId,
                CoinId = MenuInput.ReadKeepCurrentId(
                    "Coin ID",
                    current.CoinId),
                ImageType = MenuInput.ReadKeepCurrentRequiredString(
                    "Image type",
                    current.ImageType),
                FileName = MenuInput.ReadKeepCurrentRequiredString(
                    "File name",
                    current.FileName),
                FilePath = MenuInput.ReadKeepCurrentRequiredString(
                    "File path",
                    current.FilePath),
                Description = MenuInput.ReadKeepCurrentString(
                    "Description",
                    current.Description),
                SortOrder = ReadKeepCurrentSortOrder(
                    current.SortOrder)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating coin image...");

            var updated = await _coinImageService.UpdateAsync(model);

            if (updated is null)
            {
                Console.WriteLine();
                Console.WriteLine("Coin image could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Coin image updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating coin image.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Delete");
        Console.WriteLine();

        var coinImageId = MenuInput.ReadIdOrExit("Coin Image ID");

        if (coinImageId is null)
        {
            return;
        }

        try
        {
            var current = await _coinImageService.GetByIdAsync(coinImageId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Coin image with ID {coinImageId.Value} was not found.");

                Pause();
                return;
            }

            Console.WriteLine("--- Coin Image ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("This action permanently deletes the coin image.");

            Console.WriteLine();
            Console.Write("Type DELETE to confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(confirmation, "DELETE", StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Pause();
                return;
            }

            var deletedId =
                await _coinImageService.DeleteAsync(
                    new DeleteCoinImageViewModel
                    {
                        CoinImageId = coinImageId.Value
                    });

            Console.WriteLine();
            Console.WriteLine($"Coin image {deletedId} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting coin image.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Filtered lists
    // ============================================================

    private async Task ListByCoinAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Images by Coin");
        Console.WriteLine();

        var coinId = MenuInput.ReadIdOrExit("Coin ID");

        if (coinId is null)
        {
            return;
        }

        try
        {
            var images = await _coinImageService.GetByCoinAsync(coinId.Value);

            Console.WriteLine();

            if (images.Count == 0)
            {
                Console.WriteLine($"No images found for Coin ID {coinId.Value}.");
            }
            else
            {
                PrintImages(images);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading coin images.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Input helpers
    // ============================================================

    private static int ReadCreateSortOrder()
    {
        while (true)
        {
            Console.Write("Sort order [0]: ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a non-negative integer.");
        }
    }

    private static int ReadKeepCurrentSortOrder(int current)
    {
        while (true)
        {
            Console.Write($"Sort order [{current}] " + "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a non-negative integer " + "or press Enter to keep the current value.");
        }
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine("=== Coin Images ===");
        Console.WriteLine();
    }

    private static void PrintImages(IReadOnlyList<CoinImageListItemViewModel> images)
    {
        if (images.Count == 0)
        {
            Console.WriteLine("No coin images found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",5}  " +
            $"{"Coin",5}  " +
            $"{"Type",-15} " +
            $"{"File",-30} " +
            $"{"Order",5}");

        Console.WriteLine(
            new string('-', 70));

        foreach (var image in images)
        {
            Console.WriteLine(
                $"{image.CoinImageId,5}  " +
                $"{image.CoinId,5}  " +
                $"{image.ImageType,-15} " +
                $"{Truncate(image.FileName, 30),-30} " +
                $"{image.SortOrder,5}");
        }
    }

    private static void PrintDetails(
        CoinImageDetailsViewModel image)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Coin Image ID: {image.CoinImageId}");

        Console.WriteLine($"Coin ID:       {image.CoinId}");

        Console.WriteLine();

        Console.WriteLine("--- Image ---");
        Console.WriteLine($"Type:          {image.ImageType}");

        Console.WriteLine($"File Name:     {image.FileName}");

        Console.WriteLine($"File Path:     {image.FilePath}");

        Console.WriteLine();

        Console.WriteLine("--- Description ---");
        Console.WriteLine(image.Description ?? "-");

        Console.WriteLine();

        Console.WriteLine("--- Ordering ---");
        Console.WriteLine($"Sort Order:    {image.SortOrder}");

        Console.WriteLine();

        Console.WriteLine("--- Metadata ---");
        Console.WriteLine($"Created At:    {image.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC");
    }

    private static void PrintUpdateSummary(UpdateCoinImageViewModel image)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Coin Image ID: {image.CoinImageId}");

        Console.WriteLine($"Coin ID:       {image.CoinId}");

        Console.WriteLine();

        Console.WriteLine("--- Image ---");
        Console.WriteLine($"Type:          {image.ImageType}");

        Console.WriteLine($"File Name:     {image.FileName}");

        Console.WriteLine($"File Path:     {image.FilePath}");

        Console.WriteLine();

        Console.WriteLine("--- Description ---");
        Console.WriteLine(image.Description ?? "-");

        Console.WriteLine();

        Console.WriteLine("--- Ordering ---");
        Console.WriteLine($"Sort Order:    {image.SortOrder}");
    }

    private static string Truncate(string value, int maxLength)
    {
        if (value.Length <= maxLength)
        {
            return value;
        }

        return value[..(maxLength - 3)] + "...";
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}