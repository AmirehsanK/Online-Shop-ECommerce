using Domain.Enums;

namespace Domain.ViewModel.Ticket.Admin;

public class ShowAllTicketList
{
    public int Ticketid { get; set; }
    public int OwnerId { get; set; }

    public string Title { get; set; }

    public bool IsClosed { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public TicketsEnum.Section Section { get; set; }

    public TicketsEnum.Priority Priority { get; set; }

    public TicketsEnum.Status Status { get; set; }
}