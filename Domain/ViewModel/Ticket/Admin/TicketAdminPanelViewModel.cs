using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Ticket.Admin
{
    public class TicketAdminPanelViewModel
    {
        public int Ticketid { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
