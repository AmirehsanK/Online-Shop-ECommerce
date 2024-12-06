using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ViewModel.Ticket;

namespace Application.Services.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketListViewModel>> GetTicketListForShow(int userId);

        Task AddNewTicket(AddTicketViewModel ticket, int userid);

        Task<TicketDetailViewModel> GetTicketDetail(int ticketid);

        Task AddMessageToCurrentTicket(TicketDetailViewModel model,int  ticketid,int userid);

    }
}
