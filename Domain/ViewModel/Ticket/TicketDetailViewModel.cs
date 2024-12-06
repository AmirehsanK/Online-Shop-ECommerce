using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Ticket;
using Domain.Enums;

namespace Domain.ViewModel.Ticket
{
    public class TicketDetailViewModel
    {
        [Display(Name = "پیام")] 
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Message { get; set; }

        public string? FileName { get; set; }
        public Entities.Ticket.Ticket? Ticket { get; set; }
        
        public ICollection<TicketsMessage>? Messages { get; set; }
    }
}
