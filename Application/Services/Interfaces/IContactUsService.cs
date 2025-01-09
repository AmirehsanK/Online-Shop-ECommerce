using Application.DTO;
using Domain.Entities.ContactUs;
using Domain.ViewModel.ContactUs;

namespace Application.Services.Interfaces;

public interface IContactUsService
{
    
    #region Subject Management

    Task<List<Subject>> GetSubjectsAsync();

    #endregion

    #region Message Retrieval

    Task<List<ContactUsAdminViewModel>> GetMessagesForAdminAsync();
    Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync();
    Task<ContactMessageDto> GetMessageByIdAsync(int id);

    #endregion

    #region Message Management

    Task AnswerMessageAsync(int id, string messageResponse);
    Task AddMessage(ContactMessageDto dto);

    #endregion
    
}