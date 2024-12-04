using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Ticket;

namespace Domain.Interface
{
    public interface ITicketRepository
    {
        Task AddTicketAsync(Ticket  ticket);
        void UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Ticket ticket);

        Task<List<Ticket>> GetAllCurrentTicketAsync(int UserId);

        Task SaveChangeAsync();

        Task AddTicketMessageAsync(TicketsMessage ticketsMessage);

        Task DeleteTicketMessageAsync(TicketsMessage ticketsMessage);

        void UpdateTicketMessageAsync(TicketsMessage ticketsMessage);

        Task<List<TicketsMessage>> GetTicketMessageCurrentAsync(int UserId,int TicketId);

        Task<Ticket> GetTicketAsync(int ticketid);
        Task<TicketsMessage> GetTicketMessageAsync(int ticketid); 

        Task<List<Ticket>> GetAllAsync();







    }
}
