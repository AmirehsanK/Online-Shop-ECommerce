using Application.Services.Interfaces;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Impelementation;

public class AdminService(
    ITicketService ticketService,
    ICommentService commentService,
    IDiscountService discountService,
    IContactUsService contactUsService) : IAdminService
{
    #region Admin Panel

    public async Task<AdminPanelViewModel> GetAdminPanelAsync()
    {
        var discounts = await discountService.GetAllForAdminAsync();
        var contactUsList = await contactUsService.GetMessagesForAdminAsync();
        var tickets = await ticketService.GetAllTicketListForAdmin();

        return new AdminPanelViewModel
        {
            SalesAmount = 1532,
            ActiveDiscountList = discounts,
            TicketList = tickets,
            ContactUsList = contactUsList
        };
    }

    #endregion
}