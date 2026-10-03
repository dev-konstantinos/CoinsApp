using CoinsApp.UI.Helpers;
using CoinsApp.BLL.Features.Collections;
using CoinsApp.BLL.Features.Collections.ViewModels;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CollectionMenu
{
    private readonly ICollectionService _collectionService;

    public CollectionMenu(ICollectionService collectionService)
    {
        _collectionService =
            collectionService
            ?? throw new ArgumentNullException(
                nameof(collectionService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Collections ===");
            Console.WriteLine();
            Console.WriteLine("1. List collections");
            Console.WriteLine("2. Collection details");
            Console.WriteLine("3. Create collection");
            Console.WriteLine("4. Update collection");
            Console.WriteLine("5. Delete collection");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input =
                Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await ListAsync();
                    break;

                case "2":
                    await DetailsAsync();
                    break;

                case "3":
                    await CreateAsync();
                    break;

                case "4":
                    await UpdateAsync();
                    break;

                case "5":
                    await DeleteAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine();
                    Console.WriteLine(
                        "Press Enter to continue...");
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

        Console.WriteLine("=== Collections ===");
        Console.WriteLine();

        try
        {
            var collections =
                await _collectionService.GetAllAsync();

            if (collections.Count == 0)
            {
                Console.WriteLine(
                    "No collections found.");
            }
            else
            {
                Console.WriteLine(
                    $"{"ID",4}  " +
                    $"{"Name",-30} " +
                    $"{"User",-20} " +
                    $"{"Active",-7} " +
                    $"{"Coins",5}");

                Console.WriteLine(
                    new string('-', 75));

                foreach (var collection in collections)
                {
                    Console.WriteLine(
                        $"{collection.CollectionId,4}  " +
                        $"{collection.Name,-30} " +
                        $"{collection.UserName,-20} " +
                        $"{(collection.IsActive ? "Yes" : "No"),-7} " +
                        $"{collection.CoinCount,5}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading collections.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine(
            "Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Collection Details ===");
        Console.WriteLine();

        var collectionId =
            MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
        {
            return;
        }

        try
        {
            var collection =
                await _collectionService.GetByIdAsync(
                    collectionId.Value);

            if (collection is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Collection with ID " +
                    $"{collectionId.Value} was not found.");
            }
            else
            {
                Console.Clear();

                Console.WriteLine(
                    "=== Collection Details ===");
                Console.WriteLine();

                PrintDetails(collection);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error loading collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine(
            "Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Collection ===");
        Console.WriteLine();

        try
        {
            var model =
                new CreateCollectionViewModel
                {
                    UserId =
                        MenuInput.ReadRequiredId(
                            "User ID"),

                    Name =
                        ReadRequiredCollectionName(),

                    Description =
                        MenuInput.ReadNullableString(
                            "Description"),

                    IsActive = true
                };

            Console.WriteLine();
            Console.WriteLine(
                "Creating collection...");

            var collectionId =
                await _collectionService.CreateAsync(
                    model);

            Console.WriteLine();
            Console.WriteLine(
                $"Collection created successfully. " +
                $"Collection ID: {collectionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error creating collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine(
            "Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Collection ===");
        Console.WriteLine();

        var collectionId =
            MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
        {
            return;
        }

        try
        {
            var current =
                await _collectionService.GetByIdAsync(
                    collectionId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Collection with ID " +
                    $"{collectionId.Value} was not found.");

                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue...");
                Console.ReadLine();

                return;
            }

            Console.Clear();

            Console.WriteLine(
                "=== Update Collection ===");
            Console.WriteLine(
                $"Collection ID: {current.CollectionId}");
            Console.WriteLine();

            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine();

            var model =
                new UpdateCollectionViewModel
                {
                    CollectionId =
                        current.CollectionId,

                    Name =
                        ReadKeepCurrentCollectionName(
                            current.Name),

                    Description =
                        MenuInput.ReadKeepCurrentString(
                            "Description",
                            current.Description),

                    IsActive =
                        ReadKeepCurrentBoolean(
                            "Active",
                            current.IsActive)
                };

            Console.WriteLine();
            Console.WriteLine(
                "Updating collection...");

            var updated =
                await _collectionService.UpdateAsync(
                    model);

            if (updated is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Collection could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Collection updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error updating collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine(
            "Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Collection ===");
        Console.WriteLine();

        var collectionId =
            MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
        {
            return;
        }

        try
        {
            var current =
                await _collectionService.GetByIdAsync(
                    collectionId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Collection with ID " +
                    $"{collectionId.Value} was not found.");

                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue...");
                Console.ReadLine();

                return;
            }

            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.Write(
                "Type DELETE to confirm: ");

            var confirmation =
                Console.ReadLine()?.Trim();

            if (!string.Equals(
                    confirmation,
                    "DELETE",
                    StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Delete cancelled.");

                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue...");
                Console.ReadLine();

                return;
            }

            var model =
                new DeleteCollectionViewModel
                {
                    CollectionId =
                        collectionId.Value
                };

            await _collectionService.DeleteAsync(
                model);

            Console.WriteLine();
            Console.WriteLine(
                "Collection deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error deleting collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine(
            "Press Enter to continue...");
        Console.ReadLine();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintDetails(
        CollectionDetailsViewModel collection)
    {
        Console.WriteLine(
            $"Collection ID: {collection.CollectionId}");

        Console.WriteLine(
            $"User ID:       {collection.UserId}");

        Console.WriteLine(
            $"User:          {collection.UserName}");

        Console.WriteLine(
            $"Name:          {collection.Name}");

        Console.WriteLine(
            $"Description:   {collection.Description ?? "-"}");

        Console.WriteLine(
            $"Active:        " +
            $"{(collection.IsActive ? "Yes" : "No")}");

        Console.WriteLine(
            $"Coin count:    {collection.CoinCount}");

        Console.WriteLine(
            $"Created:       " +
            $"{collection.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    }

    // ============================================================
    // Input helpers
    // ============================================================

    private static string ReadRequiredCollectionName()
    {
        while (true)
        {
            Console.Write("Name: ");

            var input =
                Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                var name =
                    input.Trim();

                if (name.Length <= 150)
                {
                    return name;
                }
            }

            Console.WriteLine(
                "Please enter a collection name " +
                "with 1 to 150 characters.");
        }
    }

    private static string ReadKeepCurrentCollectionName(
        string current)
    {
        while (true)
        {
            Console.Write(
                $"Name [{current}] " +
                "(Enter = keep current): ");

            var input =
                Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            var name =
                input.Trim();

            if (name.Length <= 150)
            {
                return name;
            }

            Console.WriteLine(
                "Collection name cannot exceed 150 characters.");
        }
    }

    private static bool ReadKeepCurrentBoolean(
        string label,
        bool current)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{(current ? "Yes" : "No")}] " +
                "(Enter = keep, y = yes, n = no): ");

            var input =
                Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (input.Equals(
                    "y",
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (input.Equals(
                    "n",
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine(
                "Please enter y, n, or press Enter " +
                "to keep the current value.");
        }
    }
}