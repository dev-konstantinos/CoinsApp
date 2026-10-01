using CoinsApp.BLL.Features.Collections.ViewModels;

namespace CoinsApp.BLL.Features.Collections
{
    public interface ICollectionService
    {
        Task<int> CreateAsync(CreateCollectionViewModel model);
        Task<int> DeleteAsync(DeleteCollectionViewModel model);
        Task<IReadOnlyList<CollectionListItemViewModel>> GetAllAsync(int? userId = null);
        Task<CollectionDetailsViewModel?> GetByIdAsync(int collectionId);
        Task<CollectionDetailsViewModel?> UpdateAsync(UpdateCollectionViewModel model);
    }
}