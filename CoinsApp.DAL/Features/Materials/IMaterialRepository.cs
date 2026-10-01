using CoinsApp.DAL.Features.Materials.Models;

namespace CoinsApp.DAL.Features.Materials
{
    public interface IMaterialRepository
    {
        Task<MaterialData?> CreateAsync(MaterialCreateData data);
        Task<IReadOnlyList<MaterialData>> GetAllAsync();
        Task<MaterialData?> GetByIdAsync(int materialId);
        Task<MaterialData?> SetActiveAsync(MaterialSetActiveData data);
        Task<MaterialData?> UpdateAsync(MaterialUpdateData data);
    }
}