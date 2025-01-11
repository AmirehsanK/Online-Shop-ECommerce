namespace Domain.ViewModel.Ticket;

public class TicketMessageViewModel
{
    public int SenderId { get; set; }
    public string Message { get; set; }
    public DateTime CreateDate { get; set; }

    public string FileName { get; set; }
}