using CoinsApp.BLL.Features.Sales.ViewModels;

namespace CoinsApp.BLL.Features.Sales
{
    public interface ISaleService
    {
        Task<int> CreateAsync(CreateSaleViewModel model);
        Task<int> DeleteAsync(DeleteSaleViewModel model);
        Task<IReadOnlyList<SaleListItemViewModel>> GetByCoinAsync(int coinId);
        Task<SaleDetailsViewModel?> GetByIdAsync(int saleId);
        Task<int> UpdateAsync(UpdateSaleViewModel model);
    }
}