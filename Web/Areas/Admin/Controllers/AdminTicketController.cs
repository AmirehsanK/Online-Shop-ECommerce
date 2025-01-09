using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.TicketManagement)]
    public class AdminTicketController : AdminBaseController
    {
        #region Ctor

        private readonly ITicketService _ticketService;

        public AdminTicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region ShowAllTicket

        [InvokePermission(PermissionName.TicketList)]
        public async Task<IActionResult> AllTicketList()
        {
            var model = await _ticketService.GetAllTicketListForAdmin();
            return View(model);
        }

        #endregion

        #region AnswerTicket

        [HttpGet]
        [InvokePermission(PermissionName.AnswerTicket)]
        public async Task<IActionResult> AnswerTicket(int TicketId)
        {
            var model = await _ticketService.GetTicketDetail(TicketId);
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.AnswerTicket)]
        public async Task<IActionResult> AnswerTicket(TicketDetailViewModel Ticket, int TicketId)
        {
            if (!ModelState.IsValid)
            {
                var model = await _ticketService.GetTicketDetail(TicketId);
                return View(model);
            }

            await _ticketService.AddMessageToCurrentTicketFromAdmin(Ticket, TicketId, User.GetCurrentUserId());
            return RedirectToAction(nameof(AllTicketList));
        }

        #endregion

        #region CloseTicket

        [InvokePermission(PermissionName.DeleteTicket)]
        public async Task<IActionResult> CloseTicket(int TicketId)
        {
            await _ticketService.CloseTicket(TicketId);
            return RedirectToAction(nameof(AllTicketList));
        }

        #endregion
    }
}