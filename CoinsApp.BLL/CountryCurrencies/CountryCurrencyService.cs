using CoinsApp.BLL.CountryCurrencies.ViewModels;
using CoinsApp.DAL.CountryCurrencies;
using CoinsApp.DAL.CountryCurrencies.Models;

namespace CoinsApp.BLL.CountryCurrencies;

public sealed class CountryCurrencyService : ICountryCurrencyService
{
    private readonly ICountryCurrencyRepository _repository;

    public CountryCurrencyService(
        ICountryCurrencyRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CountryCurrencyListItemViewModel>>
        GetAllAsync()
    {
        var countryCurrencies =
            await _repository.GetAllAsync();

        return countryCurrencies
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CountryCurrencyDetailsViewModel?> GetByIdAsync(
        int countryCurrencyId)
    {
        ValidateCountryCurrencyId(countryCurrencyId);

        var countryCurrency =
            await _repository.GetByIdAsync(countryCurrencyId);

        return countryCurrency is null
            ? null
            : MapToDetails(countryCurrency);
    }

    public async Task<IReadOnlyList<CountryCurrencyListItemViewModel>>
        GetByCountryAsync(int countryId)
    {
        ValidateCountryId(countryId);

        var countryCurrencies =
            await _repository.GetByCountryAsync(countryId);

        return countryCurrencies
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<IReadOnlyList<CountryCurrencyListItemViewModel>>
        GetByCurrencyAsync(int currencyId)
    {
        ValidateCurrencyId(currencyId);

        var countryCurrencies =
            await _repository.GetByCurrencyAsync(currencyId);

        return countryCurrencies
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CountryCurrencyDetailsViewModel?> CreateAsync(
        CreateCountryCurrencyViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CountryCurrencyCreateData
        {
            CountryId = model.CountryId,
            CurrencyId = model.CurrencyId,
            IsActive = model.IsActive
        };

        var countryCurrency =
            await _repository.CreateAsync(data);

        return countryCurrency is null
            ? null
            : MapToDetails(countryCurrency);
    }

    public async Task<CountryCurrencyDetailsViewModel?> SetActiveAsync(
        SetCountryCurrencyActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCountryCurrencyId(model.CountryCurrencyId);

        var data = new CountryCurrencySetActiveData
        {
            CountryCurrencyId = model.CountryCurrencyId,
            IsActive = model.IsActive
        };

        var countryCurrency =
            await _repository.SetActiveAsync(data);

        return countryCurrency is null
            ? null
            : MapToDetails(countryCurrency);
    }

    private static void ValidateCreate(
        CreateCountryCurrencyViewModel model)
    {
        ValidateCountryId(model.CountryId);
        ValidateCurrencyId(model.CurrencyId);
    }

    private static void ValidateCountryCurrencyId(
        int countryCurrencyId)
    {
        if (countryCurrencyId <= 0)
            throw new ArgumentException(
                "CountryCurrency ID must be greater than zero.",
                nameof(countryCurrencyId));
    }

    private static void ValidateCountryId(int countryId)
    {
        if (countryId <= 0)
            throw new ArgumentException(
                "Country ID must be greater than zero.",
                nameof(countryId));
    }

    private static void ValidateCurrencyId(int currencyId)
    {
        if (currencyId <= 0)
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(currencyId));
    }

    private static CountryCurrencyListItemViewModel MapToListItem(
        CountryCurrencyData countryCurrency)
    {
        return new CountryCurrencyListItemViewModel
        {
            CountryCurrencyId =
                countryCurrency.CountryCurrencyId,

            CountryId =
                countryCurrency.CountryId,

            CountryName =
                countryCurrency.CountryName,

            CountryCode =
                countryCurrency.CountryCode,

            CurrencyId =
                countryCurrency.CurrencyId,

            CurrencyName =
                countryCurrency.CurrencyName,

            CurrencyCode =
                countryCurrency.CurrencyCode,

            CurrencySymbol =
                countryCurrency.CurrencySymbol,

            IsActive =
                countryCurrency.IsActive
        };
    }

    private static CountryCurrencyDetailsViewModel MapToDetails(
        CountryCurrencyData countryCurrency)
    {
        return new CountryCurrencyDetailsViewModel
        {
            CountryCurrencyId =
                countryCurrency.CountryCurrencyId,

            CountryId =
                countryCurrency.CountryId,

            CountryName =
                countryCurrency.CountryName,

            CountryCode =
                countryCurrency.CountryCode,

            CurrencyId =
                countryCurrency.CurrencyId,

            CurrencyName =
                countryCurrency.CurrencyName,

            CurrencyCode =
                countryCurrency.CurrencyCode,

            CurrencySymbol =
                countryCurrency.CurrencySymbol,

            IsActive =
                countryCurrency.IsActive
        };
    }
}