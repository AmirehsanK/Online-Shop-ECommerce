using Application.DTO;
using Domain.Entities.ContactUs;
using Domain.ViewModel.ContactUs;

namespace Application.Services.Interfaces;

public interface IContactUsService
{
    Task<List<Subject>> GetSubjectsAsync();
    Task<List<ContactUsAdminViewModel>> GetMessagesForAdminAsync();
    Task AnswerMessageAsync(int id, string messageResponse);
    Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync();
    Task AddMessage(ContactMessageDto dto);
    Task<ContactMessageDto> GetMessageByIdAsync(int id);
}