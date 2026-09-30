using CoinsApp.DAL.Users.Models;

namespace CoinsApp.DAL.Users
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserCreateData data);
        Task<int> DeleteAsync(UserDeleteData data);
        Task<IReadOnlyList<UserData>> GetAllAsync();
        Task<UserData?> GetByIdAsync(int userId);
        Task<int> SetPasswordAsync(UserSetPasswordData data);
        Task<UserData?> UpdateAsync(UserUpdateData data);
    }
}