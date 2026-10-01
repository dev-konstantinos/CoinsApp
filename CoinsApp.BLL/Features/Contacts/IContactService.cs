using CoinsApp.BLL.Features.Contacts.ViewModels;

namespace CoinsApp.BLL.Features.Contacts
{
    public interface IContactService
    {
        Task<int> CreateAsync(CreateContactViewModel model);
        Task<int> DeleteAsync(DeleteContactViewModel model);
        Task<IReadOnlyList<ContactListItemViewModel>> GetAllAsync();
        Task<ContactDetailsViewModel?> GetByIdAsync(int contactId);
        Task<ContactDetailsViewModel?> UpdateAsync(UpdateContactViewModel model);
    }
}