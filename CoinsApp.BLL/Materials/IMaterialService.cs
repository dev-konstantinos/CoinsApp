using CoinsApp.BLL.Materials.ViewModels;

namespace CoinsApp.BLL.Materials
{
    public interface IMaterialService
    {
        Task<MaterialDetailsViewModel?> CreateAsync(CreateMaterialViewModel model);
        Task<IReadOnlyList<MaterialListItemViewModel>> GetAllAsync();
        Task<MaterialDetailsViewModel?> GetByIdAsync(int materialId);
        Task<MaterialDetailsViewModel?> SetActiveAsync(SetMaterialActiveViewModel model);
        Task<MaterialDetailsViewModel?> UpdateAsync(UpdateMaterialViewModel model);
    }
}