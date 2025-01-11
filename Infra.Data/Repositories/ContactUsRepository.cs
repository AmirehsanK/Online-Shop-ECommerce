using Domain.Entities.ContactUs;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ContactUsRepository(ApplicationDbContext context) : IContactUsRepository
{
    #region Subject Methods

    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await context.Subjects.ToListAsync();
    }

    public async Task<Subject> GetSubjectByIdAsync(int id)
    {
        return (await context.Subjects.FirstOrDefaultAsync(idSubject => idSubject.Id == id))!;
    }

    #endregion

    #region Contact Message Methods

    public async Task AddMessageAsync(ContactMessage message)
    {
        await context.ContactMessages.AddAsync(message);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContactMessage>> GetMessagesAsync()
    {
        return await context.ContactMessages.ToListAsync();
    }

    public async Task<ContactMessage> GetMessageByIdAsync(int id)
    {
        return (await context.ContactMessages.FindAsync(id))!;
    }

    public async Task UpdateMessageAsync(ContactMessage message)
    {
        context.ContactMessages.Update(message);
        await context.SaveChangesAsync();
    }

    #endregion
}