using Domain.Entities.ContactUs;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class SubjectRepository
{
    private readonly DbContext _context;

    public SubjectRepository(DbContext context)
    {
        _context = context;
    }
    //TODO Write the code
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        //return await _context.Subjects.ToListAsync();
        throw new Exception();
    }
}