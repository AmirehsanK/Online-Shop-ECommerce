using System.ComponentModel.DataAnnotations;
using Domain.Entities.Ticket;

namespace Domain.ViewModel.Ticket;

public class TicketDetailViewModel
{
    [Display(Name = "پیام")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Message { get; set; }

    public string? FileName { get; set; }
    public TicketViewModel Ticket { get; set; }


    public List<TicketsMessage> Messages { get; set; }
}