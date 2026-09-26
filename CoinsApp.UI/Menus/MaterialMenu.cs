using CoinsApp.BLL.Materials;
using CoinsApp.BLL.Materials.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus;

internal sealed class MaterialMenu
{
    private readonly MaterialService _materialService;

    public MaterialMenu(MaterialService materialService)
    {
        _materialService =
            materialService
            ?? throw new ArgumentNullException(nameof(materialService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Materials ===");
            Console.WriteLine();
            Console.WriteLine("1. List materials");
            Console.WriteLine("2. Material details");
            Console.WriteLine("3. Create material");
            Console.WriteLine("4. Update material");
            Console.WriteLine("5. Activate / Deactivate material");
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
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
                    await SetActiveAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Pause();
                    break;
            }
        }
    }

    private async Task ListAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Materials ===");
        Console.WriteLine();

        try
        {
            var items =
                await _materialService.GetAllAsync();

            if (items.Count == 0)
            {
                Console.WriteLine("No materials found.");
                Pause();
                return;
            }

            Console.WriteLine(
                $"{"ID",4}  {"Name",-24} {"Symbol",-10} " +
                $"{"Precious",-10} {"Active",-7}");

            Console.WriteLine(new string('-', 61));

            foreach (var item in items)
            {
                Console.WriteLine(
                    $"{item.MaterialId,4}  " +
                    $"{item.Name,-24} " +
                    $"{item.Symbol ?? "-",-10} " +
                    $"{(item.IsPreciousMetal ? "Yes" : "No"),-10} " +
                    $"{(item.IsActive ? "Yes" : "No"),-7}");
            }
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

        Console.WriteLine("=== Material Details ===");
        Console.WriteLine();

        var materialId =
            MenuInput.ReadRequiredId("Material ID");

        try
        {
            var material =
                await _materialService.GetByIdAsync(materialId);

            if (material is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Material with ID {materialId} was not found.");
            }
            else
            {
                Console.WriteLine();
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

        Console.WriteLine("=== Create Material ===");
        Console.WriteLine();

        try
        {
            var model = new CreateMaterialViewModel
            {
                Name =
                    MenuInput.ReadRequiredString("Name"),

                Symbol =
                    MenuInput.ReadNullableString("Symbol"),

                IsPreciousMetal =
                    MenuInput.ReadRequiredBoolean(
                        "Is precious metal"),

                IsActive =
                    MenuInput.ReadRequiredBoolean(
                        "Active")
            };

            var material =
                await _materialService.CreateAsync(model);

            Console.WriteLine();

            if (material is null)
            {
                Console.WriteLine(
                    "Material could not be created.");
            }
            else
            {
                Console.WriteLine(
                    $"Material created successfully. " +
                    $"Material ID: {material.MaterialId}");
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

        Console.WriteLine("=== Update Material ===");
        Console.WriteLine();

        var materialId =
            MenuInput.ReadRequiredId("Material ID");

        try
        {
            var current =
                await _materialService.GetByIdAsync(materialId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Material with ID {materialId} was not found.");

                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to keep the current value.");
            Console.WriteLine(
                "Type null to clear the optional symbol.");
            Console.WriteLine();

            var model = new UpdateMaterialViewModel
            {
                MaterialId =
                    current.MaterialId,

                Name =
                    MenuInput.ReadKeepCurrentRequiredString(
                        "Name",
                        current.Name),

                Symbol =
                    MenuInput.ReadKeepCurrentString(
                        "Symbol",
                        current.Symbol),

                IsPreciousMetal =
                    MenuInput.ReadKeepCurrentBoolean(
                        "Precious metal",
                        current.IsPreciousMetal),

                IsActive =
                    MenuInput.ReadKeepCurrentBoolean(
                        "Active",
                        current.IsActive)
            };

            var material =
                await _materialService.UpdateAsync(model);

            Console.WriteLine();

            Console.WriteLine(
                material is null
                    ? "Material could not be updated."
                    : "Material updated successfully.");
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

    private async Task SetActiveAsync()
    {
        Console.Clear();

        Console.WriteLine(
            "=== Activate / Deactivate Material ===");
        Console.WriteLine();

        var materialId =
            MenuInput.ReadRequiredId("Material ID");

        try
        {
            var current =
                await _materialService.GetByIdAsync(materialId);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Material with ID {materialId} was not found.");

                Pause();
                return;
            }

            var newStatus =
                !current.IsActive;

            Console.WriteLine();
            Console.WriteLine(
                $"Material: {current.Name}");
            Console.WriteLine(
                $"Current status: " +
                $"{(current.IsActive ? "Active" : "Inactive")}");
            Console.WriteLine(
                $"New status: " +
                $"{(newStatus ? "Active" : "Inactive")}");
            Console.WriteLine();

            var confirmation =
                MenuInput.ReadRequiredString(
                    $"Type {(newStatus ? "ACTIVATE" : "DEACTIVATE")} to confirm");

            if (!string.Equals(
                    confirmation,
                    newStatus
                        ? "ACTIVATE"
                        : "DEACTIVATE",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Action was cancelled.");

                Pause();
                return;
            }

            var material =
                await _materialService.SetActiveAsync(
                    new SetMaterialActiveViewModel
                    {
                        MaterialId =
                            current.MaterialId,

                        IsActive =
                            newStatus
                    });

            Console.WriteLine();

            if (material is null)
            {
                Console.WriteLine(
                    "Material status could not be changed.");
            }
            else
            {
                Console.WriteLine(
                    newStatus
                        ? "Material activated successfully."
                        : "Material deactivated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Error changing material status.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private static void PrintDetails(
        MaterialDetailsViewModel material)
    {
        Console.WriteLine(
            $"Material ID: {material.MaterialId}");

        Console.WriteLine(
            $"Name: {material.Name}");

        Console.WriteLine(
            $"Symbol: {material.Symbol ?? "-"}");

        Console.WriteLine(
            $"Precious Metal: " +
            $"{(material.IsPreciousMetal ? "Yes" : "No")}");

        Console.WriteLine(
            $"Active: " +
            $"{(material.IsActive ? "Yes" : "No")}");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}