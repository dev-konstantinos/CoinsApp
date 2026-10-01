using CoinsApp.DAL.Features.Mints.Models;

namespace CoinsApp.DAL.Features.Mints
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