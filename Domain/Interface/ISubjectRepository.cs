using Domain.Entities.ContactUs;

namespace Domain.Interface;

public interface ISubjectRepository
{
    Task<List<Subject>> GetSubjectsAsync();
}