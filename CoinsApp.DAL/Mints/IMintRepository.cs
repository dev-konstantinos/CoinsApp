using CoinsApp.DAL.Mints.Models;

namespace CoinsApp.DAL.Mints
{
    public interface IMintRepository
    {
        Task<MintData?> CreateAsync(MintCreateData data);
        Task<IReadOnlyList<MintData>> GetAllAsync();
        Task<IReadOnlyList<MintData>> GetByCountryAsync(int countryId);
        Task<MintData?> GetByIdAsync(int mintId);
        Task<MintData?> SetActiveAsync(MintSetActiveData data);
        Task<MintData?> UpdateAsync(MintUpdateData data);
    }
}