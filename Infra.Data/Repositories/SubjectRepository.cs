using Domain.Entities.ContactUs;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class SubjectRepository(ApplicationDbContext context) : ISubjectRepository
{
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await context.Subjects.ToListAsync();
    }
}