using Domain.Entities.Ticket;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository
{

    #region Ticket Methods

    public async Task AddTicketAsync(Ticket ticket)
    {
        await context.Tickets.AddAsync(ticket);
    }

    public void UpdateTicketAsync(Ticket ticket)
    {
        context.Tickets.Update(ticket);
    }

    public async Task<List<Ticket>> GetAllCurrentTicketAsync(int UserId)
    {
        return await context.Tickets
            .Where(u => u.OwnerId == UserId)
            .ToListAsync();
    }

    public async Task<Ticket> GetTicketAsync(int ticketid)
    {
        return (await context.Tickets.FindAsync(ticketid))!;
    }

    public async Task<Ticket> GetAllAsync(int ticketid)
    {
        return (await context.Tickets
            .Where(u => u.Id == ticketid)
            .Include(u => u.TicketsMessages)
            .FirstOrDefaultAsync())!;
    }

    public async Task<List<Ticket>> GetAllTicketForAdminAsync()
    {
        return await context.Tickets.ToListAsync();
    }

    #endregion

    #region Ticket Message Methods

    public async Task AddTicketMessageAsync(TicketsMessage ticketsMessage)
    {
        await context.TicketsMessages.AddAsync(ticketsMessage);
    }

    public async Task<TicketsMessage> GetTicketMessageCurrentAsync(int TicketId)
    {
        return await context.TicketsMessages
            .Where(u => u.TicketId == TicketId)
            .Include(u => u.Ticket.Id == TicketId)
            .FirstOrDefaultAsync();
    }

    public async Task<TicketsMessage> GetTicketMessageAsync(int ticketid)
    {
        return (await context.TicketsMessages.FindAsync(ticketid))!;
    }

    public async Task<List<TicketsMessage>> GetMessages(int ticketid)
    {
        return await context.TicketsMessages
            .Where(u => u.TicketId == ticketid)
            .ToListAsync();
    }

    #endregion

    #region Save Changes

    public async Task SaveChangeAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

}