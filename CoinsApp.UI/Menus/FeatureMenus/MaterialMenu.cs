using CoinsApp.BLL.Features.Materials;
using CoinsApp.BLL.Features.Materials.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class MaterialMenu
{
    private readonly IMaterialService _materialService;

    public MaterialMenu(IMaterialService materialService)
    {
        _materialService = materialService ?? throw new ArgumentNullException(nameof(materialService));
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

            Console.WriteLine("1. List materials");
            Console.WriteLine("2. Material details");
            Console.WriteLine("3. Create material");
            Console.WriteLine("4. Update material");
            Console.WriteLine("5. Activate / Deactivate material");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Pause();
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
                    await SetActiveAsync();
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
            var materials = await _materialService.GetAllAsync();

            PrintMaterials(materials);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading materials.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Material Details");
        Console.WriteLine();

        var materialId = MenuInput.ReadIdOrExit("Material ID");

        if (materialId is null)
        {
            return;
        }

        try
        {
            var material = await _materialService.GetByIdAsync(materialId.Value);

            if (material is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Material with ID {materialId.Value} was not found.");
            }
            else
            {
                PrintDetails(material);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading material.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create Material");
        Console.WriteLine();

        try
        {
            var model = new CreateMaterialViewModel
            {
                Name = MenuInput.ReadRequiredString("Name"),
                Symbol = MenuInput.ReadNullableString("Symbol"),
                IsPreciousMetal = MenuInput.ReadRequiredBoolean("Is precious metal"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating material...");

            var material = await _materialService.CreateAsync(model);

            if (material is null)
            {
                Console.WriteLine();
                Console.WriteLine("Material could not be created.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Material created successfully.");
                Console.WriteLine($"Material ID: {material.MaterialId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating material.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update Material");
        Console.WriteLine();

        var materialId = MenuInput.ReadIdOrExit("Material ID");

        if (materialId is null)
        {
            return;
        }

        try
        {
            var current = await _materialService.GetByIdAsync(materialId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Material with ID {materialId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update Material");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine("Type null to clear the optional symbol.");
            Console.WriteLine();

            var model = new UpdateMaterialViewModel
            {
                MaterialId = current.MaterialId,
                Name = MenuInput.ReadKeepCurrentRequiredString("Name", current.Name),
                Symbol = MenuInput.ReadKeepCurrentString("Symbol", current.Symbol),
                IsPreciousMetal = MenuInput.ReadKeepCurrentBoolean("Precious metal", current.IsPreciousMetal),
                IsActive = MenuInput.ReadKeepCurrentBoolean("Active", current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating material...");

            var material = await _materialService.UpdateAsync(model);

            if (material is null)
            {
                Console.WriteLine();
                Console.WriteLine("Material could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Material updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating material.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Domain actions
    // ============================================================

    private async Task SetActiveAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Activate / Deactivate Material");
        Console.WriteLine();

        var materialId = MenuInput.ReadIdOrExit("Material ID");

        if (materialId is null)
        {
            return;
        }

        try
        {
            var current = await _materialService.GetByIdAsync(materialId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine($"Material with ID {materialId.Value} was not found.");

                Pause();
                return;
            }

            PrintStatusChange(current);

            var newStatus = !current.IsActive;
            var expectedConfirmation = newStatus ? "ACTIVATE" : "DEACTIVATE";

            Console.WriteLine();
            Console.WriteLine($"New status: {(newStatus ? "Active" : "Inactive")}");

            Console.WriteLine();

            var confirmation =
                MenuInput.ReadRequiredString($"Type {expectedConfirmation} to confirm");

            if (!string.Equals(
                    confirmation,
                    expectedConfirmation,
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Action cancelled.");

                Pause();
                return;
            }

            var model = new SetMaterialActiveViewModel
            {
                MaterialId = current.MaterialId,
                IsActive = newStatus
            };

            Console.WriteLine();
            Console.WriteLine(
                newStatus
                    ? "Activating material..."
                    : "Deactivating material...");

            var material =
                await _materialService.SetActiveAsync(model);

            if (material is null)
            {
                Console.WriteLine();
                Console.WriteLine("Material status could not be changed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    newStatus
                        ? "Material activated successfully."
                        : "Material deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing material status.");
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
        Console.WriteLine("=== Materials ===");
        Console.WriteLine();
    }

    private static void PrintMaterials(IReadOnlyList<MaterialListItemViewModel> materials)
    {
        if (materials.Count == 0)
        {
            Console.WriteLine("No materials found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Name",-24} " +
            $"{"Symbol",-10} " +
            $"{"Precious",-10} " +
            $"{"Active",-7}");

        Console.WriteLine(new string('-', 61));

        foreach (var material in materials)
        {
            Console.WriteLine(
                $"{material.MaterialId,4}  " +
                $"{material.Name,-24} " +
                $"{material.Symbol ?? "-",-10} " +
                $"{(material.IsPreciousMetal ? "Yes" : "No"),-10} " +
                $"{(material.IsActive ? "Yes" : "No"),-7}");
        }
    }

    private static void PrintDetails(
        MaterialDetailsViewModel material)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Material ID: {material.MaterialId}");

        Console.WriteLine();

        Console.WriteLine("--- Material ---");
        Console.WriteLine($"Name:            {material.Name}");
        Console.WriteLine($"Symbol:          {material.Symbol ?? "-"}");
        Console.WriteLine($"Precious Metal:  {(material.IsPreciousMetal ? "Yes" : "No")}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:          {(material.IsActive ? "Yes" : "No")}");
    }

    private static void PrintUpdateSummary(
        UpdateMaterialViewModel material)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"Material ID: {material.MaterialId}");

        Console.WriteLine();

        Console.WriteLine("--- Material ---");
        Console.WriteLine($"Name:            {material.Name}");
        Console.WriteLine($"Symbol:          {material.Symbol ?? "-"}");
        Console.WriteLine($"Precious Metal:  {(material.IsPreciousMetal ? "Yes" : "No")}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine($"Active:          {(material.IsActive ? "Yes" : "No")}");
    }

    private static void PrintStatusChange(
        MaterialDetailsViewModel material)
    {
        Console.WriteLine("--- Material ---");
        Console.WriteLine($"Material ID: {material.MaterialId}");
        Console.WriteLine($"Name:        {material.Name}");
        Console.WriteLine($"Symbol:      {material.Symbol ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Current Status ---");
        Console.WriteLine($"Active:      {(material.IsActive ? "Yes" : "No")}");
    }

    // ============================================================
    // General helpers
    // ============================================================

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}