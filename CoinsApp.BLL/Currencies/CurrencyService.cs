using CoinsApp.BLL.Currencies.ViewModels;
using CoinsApp.DAL.Currencies;
using CoinsApp.DAL.Currencies.Models;

namespace CoinsApp.BLL.Currencies;

public sealed class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _repository;

    public CurrencyService(ICurrencyRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CurrencyListItemViewModel>> GetAllAsync()
    {
        var currencies = await _repository.GetAllAsync();

        return currencies
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CurrencyDetailsViewModel?> GetByIdAsync(
        int currencyId)
    {
        ValidateCurrencyId(currencyId);

        var currency =
            await _repository.GetByIdAsync(currencyId);

        return currency is null
            ? null
            : MapToDetails(currency);
    }

    public async Task<CurrencyDetailsViewModel?> CreateAsync(
        CreateCurrencyViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CurrencyCreateData
        {
            Name = model.Name.Trim(),
            Code = model.Code.Trim(),
            Symbol = NormalizeSymbol(model.Symbol),
            IsActive = model.IsActive
        };

        var currency =
            await _repository.CreateAsync(data);

        return currency is null
            ? null
            : MapToDetails(currency);
    }

    public async Task<CurrencyDetailsViewModel?> UpdateAsync(
        UpdateCurrencyViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CurrencyUpdateData
        {
            CurrencyId = model.CurrencyId,
            Name = model.Name.Trim(),
            Code = model.Code.Trim(),
            Symbol = NormalizeSymbol(model.Symbol),
            IsActive = model.IsActive
        };

        var currency =
            await _repository.UpdateAsync(data);

        return currency is null
            ? null
            : MapToDetails(currency);
    }

    public async Task<CurrencyDetailsViewModel?> SetActiveAsync(
        SetCurrencyActiveViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCurrencyId(model.CurrencyId);

        var data = new CurrencySetActiveData
        {
            CurrencyId = model.CurrencyId,
            IsActive = model.IsActive
        };

        var currency =
            await _repository.SetActiveAsync(data);

        return currency is null
            ? null
            : MapToDetails(currency);
    }

    private static void ValidateCreate(
        CreateCurrencyViewModel model)
    {
        ValidateName(model.Name);
        ValidateCode(model.Code);
        ValidateSymbol(model.Symbol);
    }

    private static void ValidateUpdate(
        UpdateCurrencyViewModel model)
    {
        ValidateCurrencyId(model.CurrencyId);
        ValidateName(model.Name);
        ValidateCode(model.Code);
        ValidateSymbol(model.Symbol);
    }

    private static void ValidateCurrencyId(int currencyId)
    {
        if (currencyId <= 0)
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(currencyId));
    }

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Currency name is required.",
                nameof(name));

        if (name.Trim().Length > 100)
            throw new ArgumentException(
                "Currency name cannot exceed 100 characters.",
                nameof(name));
    }

    private static void ValidateCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException(
                "Currency code is required.",
                nameof(code));

        if (code.Trim().Length > 10)
            throw new ArgumentException(
                "Currency code cannot exceed 10 characters.",
                nameof(code));
    }

    private static void ValidateSymbol(string? symbol)
    {
        if (symbol is not null &&
            symbol.Trim().Length > 10)
        {
            throw new ArgumentException(
                "Currency symbol cannot exceed 10 characters.",
                nameof(symbol));
        }
    }

    private static string? NormalizeSymbol(string? symbol)
    {
        return string.IsNullOrWhiteSpace(symbol)
            ? null
            : symbol.Trim();
    }

    private static CurrencyListItemViewModel MapToListItem(
        CurrencyData currency)
    {
        return new CurrencyListItemViewModel
        {
            CurrencyId = currency.CurrencyId,
            Name = currency.Name,
            Code = currency.Code,
            Symbol = currency.Symbol,
            IsActive = currency.IsActive
        };
    }

    private static CurrencyDetailsViewModel MapToDetails(
        CurrencyData currency)
    {
        return new CurrencyDetailsViewModel
        {
            CurrencyId = currency.CurrencyId,
            Name = currency.Name,
            Code = currency.Code,
            Symbol = currency.Symbol,
            IsActive = currency.IsActive
        };
    }
}