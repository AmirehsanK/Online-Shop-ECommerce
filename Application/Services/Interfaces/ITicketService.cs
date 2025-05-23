﻿using Domain.ViewModel.Ticket;
using Domain.ViewModel.Ticket.Admin;

namespace Application.Services.Interfaces;

public interface ITicketService
{
    #region Ticket Management

    Task<List<TicketListViewModel>> GetTicketListForShow(int userId);
    Task AddNewTicket(AddTicketViewModel ticket, int userid);
    Task<TicketDetailViewModel> GetTicketDetail(int ticketid);
    Task AddMessageToCurrentTicket(TicketDetailViewModel model, int ticketid, int userid);
    Task<List<ShowAllTicketList>> GetAllTicketListForAdmin();
    Task AddMessageToCurrentTicketFromAdmin(TicketDetailViewModel model, int ticketid, int userId);
    Task CloseTicket(int ticketid);

    #endregion
}