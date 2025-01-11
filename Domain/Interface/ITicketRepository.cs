using Domain.Entities.Ticket;

namespace Domain.Interface;

public interface ITicketRepository
{

    #region Ticket Management

    Task AddTicketAsync(Ticket ticket);
    void UpdateTicketAsync(Ticket ticket);
    Task<List<Ticket>> GetAllCurrentTicketAsync(int UserId);
    Task<Ticket> GetTicketAsync(int ticketid);
    Task<Ticket> GetAllAsync(int ticketid);
    Task<List<Ticket>> GetAllTicketForAdminAsync();

    #endregion

    #region Ticket Message Management

    Task AddTicketMessageAsync(TicketsMessage ticketsMessage);
    Task<TicketsMessage> GetTicketMessageCurrentAsync(int TicketId);
    Task<TicketsMessage> GetTicketMessageAsync(int ticketid);
    Task<List<TicketsMessage>> GetMessages(int ticketid);

    #endregion

    #region Save Changes

    Task SaveChangeAsync();

    #endregion

}