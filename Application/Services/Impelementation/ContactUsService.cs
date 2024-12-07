using Application.DTO;
using Application.Services.Interfaces;
using Domain.Entities.ContactUs;
using Domain.Interface;

namespace Application.Services.Impelementation;

public class ContactUsService(IContactUsRepository contactUsRepository) : IContactUsService
{
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await contactUsRepository.GetSubjectsAsync();
    }
    public async Task AddMessage(ContactMessageDto dto)
    {
        var message = new ContactMessage
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Message = dto.Message,
            Phone = dto.Phone,
            Subject = dto.Subject,
            IsAnswered = false,
            CreatedAt = DateTime.UtcNow
        };
        await contactUsRepository.AddMessageAsync(message);
    }
    public async Task<IEnumerable<ContactMessageDto>> GetMessagesAsync()
    {
        var messages = await contactUsRepository.GetMessagesAsync();
        return messages.Select(m => new ContactMessageDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            Message = m.Message,
            Subject = m.Subject,
            Phone = m.Phone,
            IsAnswered = m.IsAnswered,
            CreatedAt = m.CreatedAt
        });
    }

    public async Task AnswerMessageAsync(int id)
    {
        var message = await contactUsRepository.GetMessageByIdAsync(id);
        if (message != null)
        {
            message.IsAnswered = true;
            await contactUsRepository.UpdateMessageAsync(message);
        }
    }
}