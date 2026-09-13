using Application.Services.Interfaces;
using Domain.Interface;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Impelementation;

public class AdminService(
    ITicketService ticketService,
    IDiscountService discountService,
    IContactUsService contactUsService,
    IOrderRepository orderRepository,
    ITransactionRepository transactionRepository) : IAdminService
{
    #region Admin Panel

    public async Task<AdminPanelViewModel> GetAdminPanelAsync()
    {
        var discounts = await discountService.GetAllForAdminAsync();
        var contactUsList = await contactUsService.GetMessagesForAdminAsync();
        var tickets = await ticketService.GetAllTicketListForAdmin();

        return new AdminPanelViewModel
        {
            // Both used to be placeholders (sales was always 1532).
            SalesAmount = await transactionRepository.GetTotalSalesAsync(),
            OrderAmount = await orderRepository.CountPaidOrdersAsync(),
            ActiveDiscountList = discounts,
            TicketList = tickets,
            ContactUsList = contactUsList
        };
    }

    #endregion
}
