using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Ticket;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class TicketRepository:ITicketRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;
        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        #endregion

        public async Task AddTicketAsync(Ticket ticket)
        {
             await _context.Tickets.AddAsync(ticket);
        }

        public void UpdateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }

        public async Task DeleteTicketAsync(Ticket ticket)
        {
             _context.Tickets.Remove(ticket);
        }

        public async Task<List<Ticket>> GetAllCurrentTicketAsync(int UserId)
        {
            return await _context.Tickets.Where(u => u.OwnerId == UserId).ToListAsync();
        }

        public async Task SaveChangeAsync()
        {
             await _context.SaveChangesAsync();
        }

        public async Task AddTicketMessageAsync(TicketsMessage ticketsMessage)
        {
            await _context.TicketsMessages.AddAsync(ticketsMessage);
        }

        public async Task DeleteTicketMessageAsync(TicketsMessage ticketsMessage)
        {
            _context.TicketsMessages.Remove(ticketsMessage);
        }

        public void UpdateTicketMessageAsync(TicketsMessage ticketsMessage)
        {
            _context.TicketsMessages.Update(ticketsMessage);
        }

        
        public async Task<List<TicketsMessage>> GetTicketMessageCurrentAsync(int UserId,int TicketId)
        {
            return await _context.TicketsMessages.Where(u => u.SenderId == UserId && u.TicketId == TicketId).ToListAsync();
        }

        public async Task<Ticket> GetTicketAsync(int ticketid)
        {
           return await _context.Tickets.FindAsync(ticketid);
        }

        public async Task<TicketsMessage> GetTicketMessageAsync(int ticketid)
        {
            return await _context.TicketsMessages.FindAsync(ticketid);
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }
    }
}
