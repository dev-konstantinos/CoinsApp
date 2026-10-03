using CoinsApp.BLL.Features.Catalogs;
using CoinsApp.BLL.Features.Catalogs.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class CatalogMenu
{
    private readonly ICatalogService _catalogService;

    public CatalogMenu(ICatalogService catalogService)
    {
        _catalogService =
            catalogService
            ?? throw new ArgumentNullException(
                nameof(catalogService));
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
            var catalogs =
                await _catalogService.GetAllAsync();

            if (catalogs.Count == 0)
            {
                Console.WriteLine("No catalogs found.");
            }
            else
            {
                Console.WriteLine(
                    $"{"ID",4}  " +
                    $"{"Name",-30} " +
                    $"{"Short Name",-15} " +
                    $"{"Publisher",-25} " +
                    $"{"Active",-7}");

                Console.WriteLine(
                    new string('-', 90));

                foreach (var catalog in catalogs)
                {
                    Console.WriteLine(
                        $"{catalog.CatalogId,4}  " +
                        $"{catalog.Name,-30} " +
                        $"{catalog.ShortName ?? "-",-15} " +
                        $"{catalog.Publisher ?? "-",-25} " +
                        $"{(catalog.IsActive ? "Yes" : "No"),-7}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalogs.");
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

        Console.WriteLine("=== Catalog Details ===");
        Console.WriteLine();

        var catalogId =
            MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
        {
            return;
        }

        try
        {
            var catalog =
                await _catalogService.GetByIdAsync(
                    catalogId.Value);

            if (catalog is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Catalog with ID {catalogId.Value} was not found.");
            }
            else
            {
                Console.Clear();

                Console.WriteLine("=== Catalog Details ===");
                Console.WriteLine();

                PrintDetails(catalog);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading catalog.");
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

        Console.WriteLine("=== Create Catalog ===");
        Console.WriteLine();

        try
        {
            var model = new CreateCatalogViewModel
            {
                Name =
                    MenuInput.ReadRequiredString("Name"),

                ShortName =
                    MenuInput.ReadNullableString("Short name"),

                Publisher =
                    MenuInput.ReadNullableString("Publisher"),

                Description =
                    MenuInput.ReadNullableString("Description"),

                IsActive =
                    MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating catalog...");

            var catalog =
                await _catalogService.CreateAsync(model);

            if (catalog is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Catalog could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Catalog created successfully. " +
                    $"Catalog ID: {catalog.CatalogId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating catalog.");
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

        Console.WriteLine("=== Update Catalog ===");
        Console.WriteLine();

        var catalogId =
            MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
        {
            return;
        }

        try
        {
            var current =
                await _catalogService.GetByIdAsync(
                    catalogId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Catalog with ID {catalogId.Value} was not found.");

                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue...");

                Console.ReadLine();
                return;
            }

            Console.Clear();

            Console.WriteLine("=== Update Catalog ===");
            Console.WriteLine(
                $"Catalog ID: {current.CatalogId}");
            Console.WriteLine();

            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateCatalogViewModel
            {
                CatalogId =
                    current.CatalogId,

                Name =
                    MenuInput.ReadKeepCurrentRequiredString(
                        "Name",
                        current.Name),

                ShortName =
                    MenuInput.ReadKeepCurrentString(
                        "Short name",
                        current.ShortName),

                Publisher =
                    MenuInput.ReadKeepCurrentString(
                        "Publisher",
                        current.Publisher),

                Description =
                    MenuInput.ReadKeepCurrentString(
                        "Description",
                        current.Description),

                IsActive =
                    MenuInput.ReadKeepCurrentBoolean(
                        "Active",
                        current.IsActive)
            };

            Console.WriteLine();
            Console.WriteLine("Updating catalog...");

            var updated =
                await _catalogService.UpdateAsync(model);

            if (updated is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Catalog could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Catalog updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating catalog.");
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

        Console.WriteLine("=== Delete Catalog ===");
        Console.WriteLine();

        var catalogId =
            MenuInput.ReadIdOrExit("Catalog ID");

        if (catalogId is null)
        {
            return;
        }

        try
        {
            var current =
                await _catalogService.GetByIdAsync(
                    catalogId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Catalog with ID {catalogId.Value} was not found.");

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
                Console.WriteLine("Delete cancelled.");
                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue...");

                Console.ReadLine();
                return;
            }

            await _catalogService.DeleteAsync(
                new DeleteCatalogViewModel
                {
                    CatalogId =
                        catalogId.Value
                });

            Console.WriteLine();
            Console.WriteLine(
                "Catalog deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting catalog.");
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

    private static void PrintDetails(
        CatalogDetailsViewModel catalog)
    {
        Console.WriteLine(
            $"ID:          {catalog.CatalogId}");

        Console.WriteLine(
            $"Name:        {catalog.Name}");

        Console.WriteLine(
            $"Short name:  {catalog.ShortName ?? "-"}");

        Console.WriteLine(
            $"Publisher:   {catalog.Publisher ?? "-"}");

        Console.WriteLine(
            $"Description: {catalog.Description ?? "-"}");

        Console.WriteLine(
            $"Status:      " +
            $"{(catalog.IsActive ? "Active" : "Inactive")}");
    }
}