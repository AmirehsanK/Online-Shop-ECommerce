using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.TicketManagement)]
    public class AdminTicketController(ITicketService ticketService) : AdminBaseController
    {

        #region ShowAllTicket

        [InvokePermission(PermissionName.TicketList)]
        public async Task<IActionResult> AllTicketList()
        {
            var model = await ticketService.GetAllTicketListForAdmin();
            return View(model);
        }

        #endregion

        #region AnswerTicket

        [HttpGet]
        [InvokePermission(PermissionName.AnswerTicket)]
        public async Task<IActionResult> AnswerTicket(int ticketId)
        {
            var model = await ticketService.GetTicketDetail(ticketId);
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.AnswerTicket)]
        public async Task<IActionResult> AnswerTicket(TicketDetailViewModel ticket, int ticketId)
        {
            if (!ModelState.IsValid)
            {
                var model = await ticketService.GetTicketDetail(ticketId);
                return View(model);
            }

            await ticketService.AddMessageToCurrentTicketFromAdmin(ticket, ticketId, User.GetCurrentUserId());
            return RedirectToAction(nameof(AllTicketList));
        }

        #endregion

        #region CloseTicket

        [InvokePermission(PermissionName.DeleteTicket)]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            await ticketService.CloseTicket(ticketId);
            return RedirectToAction(nameof(AllTicketList));
        }

        #endregion
        
    }
}