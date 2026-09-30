using CoinsApp.DAL.Collection.Models;

namespace CoinsApp.DAL.Collection
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