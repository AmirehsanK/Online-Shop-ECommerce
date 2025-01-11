using Domain.ViewModel.ContactUs;
using Domain.ViewModel.Discount;
using Domain.ViewModel.Ticket.Admin;

namespace Domain.ViewModel.User.Admin;

public class AdminPanelViewModel
{
    public int OrderAmount { get; set; }
    public int SalesAmount { get; set; }
    public List<ShowAllTicketList> TicketList { get; set; }
    public List<ContactUsAdminViewModel> ContactUsList { get; set; }
    public List<DiscountListAdminViewModel> ActiveDiscountList { get; set; }
}