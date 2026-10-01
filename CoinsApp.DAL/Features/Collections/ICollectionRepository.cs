using CoinsApp.DAL.Features.Collections.Models;

namespace CoinsApp.DAL.Features.Collections
{
    public interface ICollectionRepository
    {
        Task<int> CreateAsync(CollectionCreateData data);
        Task<int> DeleteAsync(CollectionDeleteData data);
        Task<IReadOnlyList<CollectionData>> GetAllAsync(int? userId = null);
        Task<CollectionData?> GetByIdAsync(int collectionId);
        Task<CollectionData?> UpdateAsync(CollectionUpdateData data);
    }
}