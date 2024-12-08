using Domain.Entities.ContactUs;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ContactUsRepository(ApplicationDbContext _context) : IContactUsRepository
{
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await _context.Subjects.ToListAsync();
    }
    
    public async Task<Subject> GetSubjectByIdAsync(int id)
    {
        return await _context.Subjects.FirstOrDefaultAsync(idSubject => idSubject.Id == id);
    }

    public async Task AddMessageAsync(ContactMessage message)
    {
        await _context.ContactMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContactMessage>> GetMessagesAsync()
    {
        return await _context.ContactMessages.ToListAsync();
    }

    public async Task<ContactMessage> GetMessageByIdAsync(int id)
    {
        return await _context.ContactMessages.FindAsync(id);
    }

    public async Task UpdateMessageAsync(ContactMessage message)
    {
        _context.ContactMessages.Update(message);
        await _context.SaveChangesAsync();
    }
}