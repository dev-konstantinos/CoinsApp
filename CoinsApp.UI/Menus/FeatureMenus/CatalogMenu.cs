using CoinsApp.BLL.Features.Catalogs;
using CoinsApp.BLL.Features.Catalogs.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CatalogMenu
{
    private readonly ICatalogService _catalogService;

    public CatalogMenu(ICatalogService catalogService)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Catalogs ===");
            Console.WriteLine();
            Console.WriteLine("1. List catalogs");
            Console.WriteLine("2. Catalog details");
            Console.WriteLine("3. Create catalog");
            Console.WriteLine("4. Update catalog");
            Console.WriteLine("5. Delete catalog");
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
                    await CreateAsync();
                    break;

                case 4:
                    await UpdateAsync();
                    break;

                case 5:
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

        Console.WriteLine("=== Catalogs ===");
        Console.WriteLine();

        try
        {
            var catalogs = await _catalogService.GetAllAsync();

            if (catalogs.Count == 0)
            {
                Console.WriteLine("No catalogs found.");
                Pause();
                return;
            }

            PrintHeader();

            foreach (var catalog in catalogs)
            {
                PrintCatalog(catalog);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalogs.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Catalog Details ===");
        Console.WriteLine();

        var catalogId = MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
            return;

        try
        {
            var catalog = await _catalogService.GetByIdAsync(catalogId.Value);

            if (catalog is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog with ID {catalogId.Value} was not found.");
                Pause();
                return;
            }

            PrintDetails(catalog);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create Catalog ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCatalogViewModel
            {
                Name = MenuInput.ReadRequiredString("Name"),
                ShortName = MenuInput.ReadNullableString("Short name"),
                Publisher = MenuInput.ReadNullableString("Publisher"),
                Description = MenuInput.ReadNullableString("Description"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating catalog...");

            var catalog = await _catalogService.CreateAsync(model);

            Console.WriteLine();

            if (catalog is null)
            {
                Console.WriteLine("Catalog could not be created.");
            }
            else
            {
                Console.WriteLine($"Catalog created successfully. Catalog ID: {catalog.CatalogId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating catalog.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update Catalog ===");
        Console.WriteLine();

        var catalogId = MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
            return;

        try
        {
            var current = await _catalogService.GetByIdAsync(catalogId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog with ID {catalogId.Value} was not found.");
                Pause();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Catalog ===");
            Console.WriteLine();

            Console.WriteLine("--- Current Catalog ---");
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("--- Enter New Values ---");
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCatalogViewModel
            {
                CatalogId = current.CatalogId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                ShortName = MenuInput.ReadKeepCurrentString("Short name", current.ShortName),
                Publisher = MenuInput.ReadKeepCurrentString("Publisher", current.Publisher),
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
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Updating catalog...");

            var updated = await _catalogService.UpdateAsync(model);

            Console.WriteLine();

            if (updated is null)
            {
                Console.WriteLine("Catalog could not be updated.");
            }
            else
            {
                Console.WriteLine($"Catalog {updated.CatalogId} updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating catalog.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Catalog ===");
        Console.WriteLine();

        var catalogId = MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
            return;

        try
        {
            var current = await _catalogService.GetByIdAsync(catalogId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Catalog with ID {catalogId.Value} was not found.");
                Pause();
                return;
            }

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("=== WARNING ===");
            Console.WriteLine();
            Console.WriteLine("Deleting this catalog is permanent.");
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

            await _catalogService.DeleteAsync(new DeleteCatalogViewModel
            {
                CatalogId = catalogId.Value
            });

            Console.WriteLine();
            Console.WriteLine($"Catalog {catalogId.Value} deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting catalog.");
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
        Console.WriteLine($"{"ID",4}  {"Name",-30} {"Short Name",-15} {"Publisher",-25} {"Active",-7}");
        Console.WriteLine(new string('-', 90));
    }

    private static void PrintCatalog(CatalogListItemViewModel catalog)
    {
        Console.WriteLine($"{catalog.CatalogId,4}  {catalog.Name,-30} {catalog.ShortName ?? "-",-15} {catalog.Publisher ?? "-",-25} {(catalog.IsActive ? "Yes" : "No"),-7}");
    }

    private static void PrintDetails(CatalogDetailsViewModel catalog)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Catalog ID:    {catalog.CatalogId}");
        Console.WriteLine($"Name:          {catalog.Name}");
        Console.WriteLine($"Short Name:    {catalog.ShortName ?? "-"}");
        Console.WriteLine($"Publisher:     {catalog.Publisher ?? "-"}");

        Console.WriteLine();
        Console.WriteLine("--- Description ---");
        Console.WriteLine(catalog.Description ?? "-");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(catalog.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(UpdateCatalogViewModel model)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Catalog ID:    {model.CatalogId}");
        Console.WriteLine($"Name:          {model.Name}");
        Console.WriteLine($"Short Name:    {model.ShortName ?? "null"}");
        Console.WriteLine($"Publisher:     {model.Publisher ?? "null"}");

        Console.WriteLine();
        Console.WriteLine("--- Description ---");
        Console.WriteLine(model.Description ?? "null");

        Console.WriteLine();
        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:        {(model.IsActive ? "Yes" : "No")}");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}