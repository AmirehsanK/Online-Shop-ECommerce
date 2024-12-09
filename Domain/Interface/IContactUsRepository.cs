using Domain.Entities.ContactUs;

namespace Domain.Interface;

public interface IContactUsRepository
{
    #region Subject

    Task<List<Subject>> GetSubjectsAsync();

    Task<Subject> GetSubjectByIdAsync(int id);
        #endregion
    #region Main

    Task AddMessageAsync(ContactMessage message);
    Task<IEnumerable<ContactMessage>> GetMessagesAsync();
    Task<ContactMessage> GetMessageByIdAsync(int id);
    Task UpdateMessageAsync(ContactMessage message);

    #endregion
}