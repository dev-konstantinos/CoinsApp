using CoinsApp.BLL.Features.Countries.ViewModels;
using CoinsApp.DAL.Features.Countries;
using CoinsApp.DAL.Features.Countries.Models;

namespace CoinsApp.BLL.Features.Countries;

public sealed class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;

    public CountryService(ICountryRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CountryListItemViewModel>> GetAllAsync()
    {
        var countries = await _repository.GetAllAsync();

        return countries
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CountryDetailsViewModel?> GetByIdAsync(
        int countryId)
    {
        ValidateCountryId(countryId);

        var country =
            await _repository.GetByIdAsync(countryId);

        return country is null
            ? null
            : MapToDetails(country);
    }

    public async Task<CountryDetailsViewModel?> CreateAsync(
        CreateCountryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CountryCreateData
        {
            Name = model.Name.Trim(),
            Code = model.Code.Trim(),
            IsActive = model.IsActive
        };

        var country =
            await _repository.CreateAsync(data);

        return country is null
            ? null
            : MapToDetails(country);
    }

    public async Task<CountryDetailsViewModel?> UpdateAsync(
        UpdateCountryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CountryUpdateData
        {
            CountryId = model.CountryId,
            Name = model.Name.Trim(),
            Code = model.Code.Trim(),
            IsActive = model.IsActive
        };

        var country =
            await _repository.UpdateAsync(data);

        return country is null
            ? null
            : MapToDetails(country);
    }

    public async Task<CountryDetailsViewModel?> SetActiveAsync(
        SetCountryActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCountryId(model.CountryId);

        var data = new CountrySetActiveData
        {
            CountryId = model.CountryId,
            IsActive = model.IsActive
        };

        var country =
            await _repository.SetActiveAsync(data);

        return country is null
            ? null
            : MapToDetails(country);
    }

    private static void ValidateCreate(
        CreateCountryViewModel model)
    {
        ValidateName(model.Name);
        ValidateCode(model.Code);
    }

    private static void ValidateUpdate(
        UpdateCountryViewModel model)
    {
        ValidateCountryId(model.CountryId);
        ValidateName(model.Name);
        ValidateCode(model.Code);
    }

    private static void ValidateCountryId(int countryId)
    {
        if (countryId <= 0)
            throw new ArgumentException(
                "Country ID must be greater than zero.",
                nameof(countryId));
    }

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Country name is required.",
                nameof(name));

        if (name.Trim().Length > 100)
            throw new ArgumentException(
                "Country name cannot exceed 100 characters.",
                nameof(name));
    }

    private static void ValidateCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException(
                "Country code is required.",
                nameof(code));

        if (code.Trim().Length > 10)
            throw new ArgumentException(
                "Country code cannot exceed 10 characters.",
                nameof(code));
    }

    private static CountryListItemViewModel MapToListItem(
        CountryData country)
    {
        return new CountryListItemViewModel
        {
            CountryId = country.CountryId,
            Name = country.Name,
            Code = country.Code,
            IsActive = country.IsActive
        };
    }

    private static CountryDetailsViewModel MapToDetails(
        CountryData country)
    {
        return new CountryDetailsViewModel
        {
            CountryId = country.CountryId,
            Name = country.Name,
            Code = country.Code,
            IsActive = country.IsActive
        };
    }
}