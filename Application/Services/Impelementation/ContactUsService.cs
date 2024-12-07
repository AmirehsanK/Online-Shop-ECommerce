using Application.Services.Interfaces;
using Domain.Entities.ContactUs;
using Domain.Interface;

namespace Application.Services.Impelementation;

public class ContactUsService(ISubjectRepository subjectRepository) : IContactUsService
{
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await subjectRepository.GetSubjectsAsync();
    }
}