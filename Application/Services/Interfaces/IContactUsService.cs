using Application.DTO;
using Domain.Entities.ContactUs;

namespace Application.Services.Interfaces;

public interface IContactUsService
{
    Task<List<Subject>> GetSubjectsAsync();

    Task AnswerMessageAsync(int id);
    Task<IEnumerable<ContactMessageDto>> GetMessagesAsync();
    Task AddMessage(ContactMessageDto dto);
}