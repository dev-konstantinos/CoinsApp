using CoinsApp.BLL.Sale.ViewModels;

namespace CoinsApp.BLL.Sale
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