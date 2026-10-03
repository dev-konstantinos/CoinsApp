using CoinsApp.BLL.Features.CatalogEntries;
using CoinsApp.BLL.Features.CatalogEntries.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CatalogEntryMenu
{
    private readonly ICatalogEntryService _catalogEntryService;

    public CatalogEntryMenu(ICatalogEntryService catalogEntryService)
    {
        _catalogEntryService = catalogEntryService ?? throw new ArgumentNullException(nameof(catalogEntryService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Catalog Entries ===");
            Console.WriteLine();
            Console.WriteLine("1. List catalog entries");
            Console.WriteLine("2. Entry details");
            Console.WriteLine("3. List entries by Coin");
            Console.WriteLine("4. List entries by Catalog");
            Console.WriteLine("5. Create catalog entry");
            Console.WriteLine("6. Update catalog entry");
            Console.WriteLine("7. Delete catalog entry");
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
                    await ListByCatalogAsync();
                    break;

                case 5:
                    await CreateAsync();
                    break;

                case 6:
                    await UpdateAsync();
                    break;

                case 7:
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

        Console.WriteLine("=== Catalog Entries ===");
        Console.WriteLine();

        try
        {
            var entries = await _catalogEntryService.GetAllAsync();

            if (entries.Count == 0)
            {
                Console.WriteLine("No catalog entries found.");
                Pause();
                return;
            }

            PrintHeader();

            foreach (var entry in entries)
            {
                PrintEntry(entry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog entries.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Catalog Entry Details ===");
        Console.WriteLine();

        var catalogEntryId = MenuInput.ReadIdOrExit("Catalog Entry ID");

        if (catalogEntryId is null)
            return;

        try
        {
            var entry = await _catalogEntryService.GetByIdAsync(catalogEntryId.Value);

            if (entry is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog entry with ID {catalogEntryId.Value} was not found.");
                Pause();
                return;
            }

            PrintDetails(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog entry.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Catalog Entry ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCatalogEntryViewModel
            {
                CatalogId = MenuInput.ReadRequiredId("Catalog ID"),
                CoinId = MenuInput.ReadRequiredId("Coin ID"),
                CatalogNumber = MenuInput.ReadRequiredString("Catalog number"),
                Notes = MenuInput.ReadNullableString("Notes")
            };

            Console.WriteLine();
            Console.WriteLine("Creating catalog entry...");

            var entry = await _catalogEntryService.CreateAsync(model);

            Console.WriteLine();

            if (entry is null)
            {
                Console.WriteLine("Catalog entry could not be created.");
            }
            else
            {
                Console.WriteLine("Catalog entry created successfully.");
                Console.WriteLine($"Catalog Entry ID: {entry.CatalogEntryId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating catalog entry.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Catalog Entry ===");
        Console.WriteLine();

        var catalogEntryId = MenuInput.ReadIdOrExit("Catalog Entry ID");

        if (catalogEntryId is null)
            return;

        try
        {
            var current = await _catalogEntryService.GetByIdAsync(catalogEntryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog entry with ID {catalogEntryId.Value} was not found.");
                Pause();
                return;
            }

            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCatalogEntryViewModel
            {
                CatalogEntryId = current.CatalogEntryId,
                CatalogId = MenuInput.ReadKeepCurrentId("Catalog ID", current.CatalogId),
                CoinId = MenuInput.ReadKeepCurrentId("Coin ID", current.CoinId),
                CatalogNumber = MenuInput.ReadKeepCurrentRequiredString("Catalog number", current.CatalogNumber),
                Notes = MenuInput.ReadKeepCurrentString("Notes", current.Notes)
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
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Updating catalog entry...");

            var updated = await _catalogEntryService.UpdateAsync(model);

            Console.WriteLine();

            if (updated is null)
            {
                Console.WriteLine("Catalog entry could not be updated.");
            }
            else
            {
                Console.WriteLine("Catalog entry updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating catalog entry.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Catalog Entry ===");
        Console.WriteLine();

        var catalogEntryId = MenuInput.ReadIdOrExit("Catalog Entry ID");

        if (catalogEntryId is null)
            return;

        try
        {
            var current = await _catalogEntryService.GetByIdAsync(catalogEntryId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog entry with ID {catalogEntryId.Value} was not found.");
                Pause();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Delete Catalog Entry ===");
            Console.WriteLine();

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("=== WARNING ===");
            Console.WriteLine();
            Console.WriteLine("Deleting this catalog entry is permanent.");
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

            await _catalogEntryService.DeleteAsync(new DeleteCatalogEntryViewModel
            {
                CatalogEntryId = current.CatalogEntryId
            });

            Console.WriteLine();
            Console.WriteLine($"Catalog entry {current.CatalogEntryId} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting catalog entry.");
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

        Console.WriteLine("=== Catalog Entries by Coin ===");
        Console.WriteLine();

        var coinId = MenuInput.ReadIdOrExit("Coin ID");

        if (coinId is null)
            return;

        try
        {
            var entries = await _catalogEntryService.GetByCoinAsync(coinId.Value);

            if (entries.Count == 0)
            {
                Console.WriteLine("No catalog entries found.");
                Pause();
                return;
            }

            PrintHeader();

            foreach (var entry in entries)
            {
                PrintEntry(entry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog entries.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task ListByCatalogAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Catalog Entries by Catalog ===");
        Console.WriteLine();

        var catalogId = MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
            return;

        try
        {
            var entries = await _catalogEntryService.GetByCatalogAsync(catalogId.Value);

            if (entries.Count == 0)
            {
                Console.WriteLine("No catalog entries found.");
                Pause();
                return;
            }

            PrintHeader();

            foreach (var entry in entries)
            {
                PrintEntry(entry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog entries.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine($"{"ID",4}  {"Catalog",8}  {"Coin",6}  {"Catalog Number",-20} {"Notes",-30}");
        Console.WriteLine(new string('-', 80));
    }

    private static void PrintEntry(CatalogEntryListItemViewModel entry)
    {
        Console.WriteLine($"{entry.CatalogEntryId,4}  {entry.CatalogId,8}  {entry.CoinId,6}  {entry.CatalogNumber,-20} {entry.Notes ?? "-",-30}");
    }

    private static void PrintDetails(CatalogEntryDetailsViewModel entry)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Catalog Entry ID: {entry.CatalogEntryId}");
        Console.WriteLine($"Catalog ID:       {entry.CatalogId}");
        Console.WriteLine($"Coin ID:          {entry.CoinId}");
        Console.WriteLine($"Catalog Number:   {entry.CatalogNumber}");

        Console.WriteLine();
        Console.WriteLine("--- Notes ---");
        Console.WriteLine(entry.Notes ?? "-");
    }

    private static void PrintUpdateSummary(UpdateCatalogEntryViewModel model)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Catalog Entry ID: {model.CatalogEntryId}");
        Console.WriteLine($"Catalog ID:       {model.CatalogId}");
        Console.WriteLine($"Coin ID:          {model.CoinId}");
        Console.WriteLine($"Catalog Number:   {model.CatalogNumber}");

        Console.WriteLine();
        Console.WriteLine("--- Notes ---");
        Console.WriteLine(model.Notes ?? "null");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}