using CoinsApp.BLL.Features.Collections;
using CoinsApp.BLL.Features.Collections.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CollectionMenu
{
    private readonly ICollectionService _collectionService;

    public CollectionMenu(ICollectionService collectionService)
    {
        _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
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

            var input = Console.ReadLine()?.Trim();

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
            var collections = await _collectionService.GetAllAsync();

            if (collections.Count == 0)
            {
                Console.WriteLine("No collections found.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            PrintHeader();

            foreach (var collection in collections)
            {
                PrintCollection(collection);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading collections.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Collection Details ===");
        Console.WriteLine();

        var collectionId = MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
            return;

        try
        {
            var collection = await _collectionService.GetByIdAsync(collectionId.Value);

            if (collection is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Collection with ID {collectionId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            PrintDetails(collection);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Collection ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCollectionViewModel
            {
                UserId = MenuInput.ReadRequiredId("User ID"),
                Name = MenuInput.ReadRequiredString("Name"),
                Description = MenuInput.ReadNullableString("Description"),
                IsActive = true
            };

            Console.WriteLine();
            Console.WriteLine("Creating collection...");

            var collectionId = await _collectionService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine($"Collection created successfully. Collection ID: {collectionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Collection ===");
        Console.WriteLine();

        var collectionId = MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
            return;

        try
        {
            var current = await _collectionService.GetByIdAsync(collectionId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Collection with ID {collectionId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Collection ===");
            Console.WriteLine();

            Console.WriteLine("--- Current Collection ---");
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("--- Enter New Values ---");
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCollectionViewModel
            {
                CollectionId = current.CollectionId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                Description = MenuInput.ReadKeepCurrentString("Description", current.Description),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            Console.WriteLine("=== Update Preview ===");
            Console.WriteLine();

            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.Write("Save changes? (y/n): ");

            var confirmation = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirmation != "y" && confirmation != "yes")
            {
                Console.WriteLine();
                Console.WriteLine("Update cancelled.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Updating collection...");

            var updated = await _collectionService.UpdateAsync(model);

            Console.WriteLine();

            if (updated is null)
            {
                Console.WriteLine($"Collection with ID {collectionId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Collection {updated.CollectionId} updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Collection ===");
        Console.WriteLine();

        var collectionId = MenuInput.ReadIdOrExit("Collection ID");

        if (collectionId is null)
            return;

        try
        {
            var current = await _collectionService.GetByIdAsync(collectionId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Collection with ID {collectionId.Value} was not found.");
                Console.ReadLine();
                return;
            }

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("=== WARNING ===");
            Console.WriteLine();
            Console.WriteLine("Deleting this collection is permanent.");
            Console.WriteLine();

            Console.Write("Type DELETE to confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(confirmation, "DELETE", StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Console.ReadLine();
                return;
            }

            await _collectionService.DeleteAsync(new DeleteCollectionViewModel
            {
                CollectionId = collectionId.Value
            });

            Console.WriteLine();
            Console.WriteLine($"Collection {collectionId.Value} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting collection.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine($"{"ID",4}  {"Name",-30} {"User",-20} {"Active",-7} {"Coins",5}");
        Console.WriteLine(new string('-', 75));
    }

    private static void PrintCollection(CollectionListItemViewModel collection)
    {
        Console.WriteLine($"{collection.CollectionId,4}  {collection.Name,-30} {collection.UserName,-20} {(collection.IsActive ? "Yes" : "No"),-7} {collection.CoinCount,5}");
    }

    private static void PrintDetails(CollectionDetailsViewModel collection)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Collection ID: {collection.CollectionId}");
        Console.WriteLine($"User ID:       {collection.UserId}");
        Console.WriteLine($"User:          {collection.UserName}");
        Console.WriteLine($"Name:          {collection.Name}");

        Console.WriteLine();
        Console.WriteLine("--- Description ---");
        Console.WriteLine(collection.Description ?? "-");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(collection.IsActive ? "Yes" : "No")}");

        Console.WriteLine();
        Console.WriteLine("--- Statistics ---");
        Console.WriteLine($"Coin count:    {collection.CoinCount}");

        Console.WriteLine();
        Console.WriteLine("--- Dates ---");
        Console.WriteLine($"Created:       {collection.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    }

    private static void PrintUpdateSummary(UpdateCollectionViewModel model)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Collection ID: {model.CollectionId}");
        Console.WriteLine($"Name:          {model.Name}");

        Console.WriteLine();
        Console.WriteLine("--- Description ---");
        Console.WriteLine(model.Description ?? "null");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(model.IsActive ? "Yes" : "No")}");
    }
}