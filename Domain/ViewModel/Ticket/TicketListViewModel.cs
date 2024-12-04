using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.ViewModel.Ticket
{
    public class TicketListViewModel
    {
        public int  TicketId { get; set; }

        public string Title { get; set; }

        public DateTime CreateDate { get; set; }

        public TicketsEnum.Section Section { get; set; }

        public TicketsEnum.Status Status { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
