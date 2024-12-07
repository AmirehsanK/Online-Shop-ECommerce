using Domain.Entities.ContactUs;

namespace Application.Services.Interfaces;

public interface IContactUsService
{
    Task<List<Subject>> GetSubjectsAsync();
}