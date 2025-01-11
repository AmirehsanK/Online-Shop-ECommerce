using Domain.Enums;

namespace Domain.ViewModel.Ticket;

public class TicketViewModel
{
    public int TicketId { get; set; }

    public string Title { get; set; }

    public int OwnerID { get; set; }

    public TicketsEnum.Status Status { get; set; }
}