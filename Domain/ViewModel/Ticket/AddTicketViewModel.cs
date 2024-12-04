using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace Domain.ViewModel.Ticket
{
    public class AddTicketViewModel
    {
        
        public int UserId { get; set; }
        public string Title { get; set; }
        public TicketsEnum.Section Section { get; set; }
        public TicketsEnum.Priority Priority { get; set; }
        public string Message { get; set; }

        

        public IFormFile Image { get; set; }

    }
}
