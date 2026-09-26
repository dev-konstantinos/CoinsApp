using CoinsApp.BLL.Materials.ViewModels;
using CoinsApp.DAL.Materials;
using CoinsApp.DAL.Materials.Models;

namespace CoinsApp.BLL.Materials;

public sealed class MaterialService
{
    private readonly MaterialRepository _repository;

    public MaterialService(MaterialRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<MaterialListItemViewModel>>
        GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<MaterialDetailsViewModel?>
        GetByIdAsync(int materialId)
    {
        var data =
            await _repository.GetByIdAsync(materialId);

        return data is null
            ? null
            : MapToDetails(data);
    }

    public async Task<MaterialDetailsViewModel?>
        CreateAsync(CreateMaterialViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MaterialCreateData
        {
            Name = model.Name,
            Symbol = model.Symbol,
            IsPreciousMetal = model.IsPreciousMetal,
            IsActive = model.IsActive
        };

        var result =
            await _repository.CreateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<MaterialDetailsViewModel?>
        UpdateAsync(UpdateMaterialViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MaterialUpdateData
        {
            MaterialId = model.MaterialId,
            Name = model.Name,
            Symbol = model.Symbol,
            IsPreciousMetal = model.IsPreciousMetal,
            IsActive = model.IsActive
        };

        var result =
            await _repository.UpdateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<MaterialDetailsViewModel?>
        SetActiveAsync(SetMaterialActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MaterialSetActiveData
        {
            MaterialId = model.MaterialId,
            IsActive = model.IsActive
        };

        var result =
            await _repository.SetActiveAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    private static MaterialListItemViewModel
        MapToListItem(MaterialData data)
    {
        return new MaterialListItemViewModel
        {
            MaterialId = data.MaterialId,
            Name = data.Name,
            Symbol = data.Symbol,
            IsPreciousMetal = data.IsPreciousMetal,
            IsActive = data.IsActive
        };
    }

    private static MaterialDetailsViewModel
        MapToDetails(MaterialData data)
    {
        return new MaterialDetailsViewModel
        {
            MaterialId = data.MaterialId,
            Name = data.Name,
            Symbol = data.Symbol,
            IsPreciousMetal = data.IsPreciousMetal,
            IsActive = data.IsActive
        };
    }
}