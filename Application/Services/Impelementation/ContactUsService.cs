using Application.DTO;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.ContactUs;
using Domain.Interface;
using Domain.ViewModel.ContactUs;

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
    public async Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync()
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
            CreatedAt = m.CreatedAt,
            RespondedAt = m.RespondedAt
        });
    }public async Task<List<ContactUsAdminViewModel>> GetMessagesForAdminAsync()
    {
        var messages = await contactUsRepository.GetMessagesAsync();
        return messages.Select(m => new ContactUsAdminViewModel
        {
            Id = m.Id,
            FullName = m.FullName,
            Subject = m.Subject,
            IsAnswered = m.IsAnswered
        }).Take(3).ToList();
    }

    public async Task AnswerMessageAsync(int id,string messageResponse)
    {
        var message = await contactUsRepository.GetMessageByIdAsync(id);
        if (message != null)
        {
            message.IsAnswered = true;
            message.AdminResponse = messageResponse;
            message.RespondedAt = DateTime.UtcNow;
            await contactUsRepository.UpdateMessageAsync(message);
            await EmailSender.SendEmail(message.Email, message.Subject, messageResponse);
        }
    }
    
    public async Task<ContactMessageDto> GetMessageByIdAsync(int id)
    {
        var message = await contactUsRepository.GetMessageByIdAsync(id);
        
        if (message ==null) return new ContactMessageDto
        {
            Id = 0,
            Email = "not found",
            Message = "not found",
            IsAnswered =false,
            CreatedAt = DateTime.UtcNow,
        };
        return new ContactMessageDto
        {
            Id = message.Id,
            Phone = message.Phone,
            FullName = message.FullName,
            Subject = message.Subject,
            Email = message.Email,
            Message = message.Message,
            IsAnswered = message.IsAnswered,
            AdminResponse = message.AdminResponse,
            CreatedAt = message.CreatedAt,
            RespondedAt = message.RespondedAt
        };
    }
}