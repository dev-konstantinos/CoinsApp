using CoinsApp.DAL.Features.Contacts.Models;

namespace CoinsApp.DAL.Features.Contacts
{
    public interface IContactRepository
    {
        Task<int> CreateAsync(ContactCreateData data);
        Task<int> DeleteAsync(ContactDeleteData data);
        Task<IReadOnlyList<ContactData>> GetAllAsync();
        Task<ContactData?> GetByIdAsync(int contactId);
        Task<ContactData?> UpdateAsync(ContactUpdateData data);
    }
}