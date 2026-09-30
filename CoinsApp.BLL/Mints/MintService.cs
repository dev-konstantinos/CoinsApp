using CoinsApp.BLL.Mints.ViewModels;
using CoinsApp.DAL.Mints;
using CoinsApp.DAL.Mints.Models;

namespace CoinsApp.BLL.Mints;

public sealed class MintService : IMintService
{
    private readonly IMintRepository _mintRepository;

    public MintService(IMintRepository mintRepository)
    {
        _mintRepository =
            mintRepository
            ?? throw new ArgumentNullException(nameof(mintRepository));
    }

    public async Task<IReadOnlyList<MintListItemViewModel>> GetAllAsync()
    {
        var data = await _mintRepository.GetAllAsync();

        return data
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<IReadOnlyList<MintListItemViewModel>> GetByCountryAsync(
        int countryId)
    {
        var data =
            await _mintRepository.GetByCountryAsync(countryId);

        return data
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<MintDetailsViewModel?> GetByIdAsync(
        int mintId)
    {
        var data =
            await _mintRepository.GetByIdAsync(mintId);

        return data is null
            ? null
            : MapToDetails(data);
    }

    public async Task<MintDetailsViewModel?> CreateAsync(
        CreateMintViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MintCreateData
        {
            CountryId = model.CountryId,
            Name = model.Name,
            Code = model.Code,
            City = model.City
        };

        var result =
            await _mintRepository.CreateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<MintDetailsViewModel?> UpdateAsync(
        UpdateMintViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MintUpdateData
        {
            MintId = model.MintId,
            CountryId = model.CountryId,
            Name = model.Name,
            Code = model.Code,
            City = model.City,
            IsActive = model.IsActive
        };

        var result =
            await _mintRepository.UpdateAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    public async Task<MintDetailsViewModel?> SetActiveAsync(
        SetMintActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new MintSetActiveData
        {
            MintId = model.MintId,
            IsActive = model.IsActive
        };

        var result =
            await _mintRepository.SetActiveAsync(data);

        return result is null
            ? null
            : MapToDetails(result);
    }

    private static MintListItemViewModel MapToListItem(
        MintData data)
    {
        return new MintListItemViewModel
        {
            MintId = data.MintId,
            CountryId = data.CountryId,
            CountryName = data.CountryName,
            CountryCode = data.CountryCode,
            Name = data.Name,
            Code = data.Code,
            City = data.City,
            IsActive = data.IsActive
        };
    }

    private static MintDetailsViewModel MapToDetails(
        MintData data)
    {
        return new MintDetailsViewModel
        {
            MintId = data.MintId,
            CountryId = data.CountryId,
            CountryName = data.CountryName,
            CountryCode = data.CountryCode,
            Name = data.Name,
            Code = data.Code,
            City = data.City,
            IsActive = data.IsActive
        };
    }
}