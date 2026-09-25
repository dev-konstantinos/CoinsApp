using CoinsApp.BLL.Denominations.ViewModels;
using CoinsApp.DAL.Denominations;
using CoinsApp.DAL.Denominations.Models;

namespace CoinsApp.BLL.Denominations;

public sealed class DenominationService
{
    private readonly DenominationRepository _repository;

    public DenominationService(DenominationRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<DenominationListItemViewModel>>
        GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<IReadOnlyList<DenominationListItemViewModel>>
        GetByCurrencyAsync(int currencyId)
    {
        var data = await _repository.GetByCurrencyAsync(currencyId);

        return data
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<DenominationDetailsViewModel?>
        GetByIdAsync(int denominationId)
    {
        var data =
            await _repository.GetByIdAsync(denominationId);

        return data is null
            ? null
            : MapToDetails(data);
    }

    public async Task<DenominationDetailsViewModel?>
        CreateAsync(CreateDenominationViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new DenominationCreateData
        {
            CurrencyId = model.CurrencyId,
            Value = model.Value,
            DisplayName = model.DisplayName,
            IsActive = model.IsActive
        };

        var result =
            await _repository.CreateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<DenominationDetailsViewModel?>
        UpdateAsync(UpdateDenominationViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new DenominationUpdateData
        {
            DenominationId = model.DenominationId,
            CurrencyId = model.CurrencyId,
            Value = model.Value,
            DisplayName = model.DisplayName,
            IsActive = model.IsActive
        };

        var result =
            await _repository.UpdateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<DenominationDetailsViewModel?>
        SetActiveAsync(SetDenominationActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new DenominationSetActiveData
        {
            DenominationId = model.DenominationId,
            IsActive = model.IsActive
        };

        var result =
            await _repository.SetActiveAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    private static DenominationListItemViewModel
        MapToListItem(DenominationData data)
    {
        return new DenominationListItemViewModel
        {
            DenominationId = data.DenominationId,
            CurrencyId = data.CurrencyId,
            CurrencyName = data.CurrencyName,
            CurrencyCode = data.CurrencyCode,
            Value = data.Value,
            DisplayName = data.DisplayName,
            IsActive = data.IsActive
        };
    }

    private static DenominationDetailsViewModel
        MapToDetails(DenominationData data)
    {
        return new DenominationDetailsViewModel
        {
            DenominationId = data.DenominationId,
            CurrencyId = data.CurrencyId,
            CurrencyName = data.CurrencyName,
            CurrencyCode = data.CurrencyCode,
            Value = data.Value,
            DisplayName = data.DisplayName,
            IsActive = data.IsActive
        };
    }
}