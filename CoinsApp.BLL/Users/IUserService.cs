using CoinsApp.BLL.Users.ViewModels;

namespace CoinsApp.BLL.Users
{
    public interface IUserService
    {
        Task<int> CreateAsync(CreateUserViewModel model);
        Task<int> DeleteAsync(DeleteUserViewModel model);
        Task<IReadOnlyList<UserListItemViewModel>> GetAllAsync();
        Task<UserDetailsViewModel?> GetByIdAsync(int userId);
        Task<int> SetPasswordAsync(SetUserPasswordViewModel model);
        Task<UserDetailsViewModel?> UpdateAsync(UpdateUserViewModel model);
    }
}