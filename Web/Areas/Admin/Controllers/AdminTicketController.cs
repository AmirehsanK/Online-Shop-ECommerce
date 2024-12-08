using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class AdminTicketController : AdminBaseController
    {
        #region Ctor

        private readonly ITicketService _ticketService;

        public AdminTicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region ShowAllticket

        public async Task<IActionResult> AllTicketList()
        {
            var model= await _ticketService.GetAllTicketListForAdmin();
            return View(model);
        }

        #endregion

        #region MyRegion

        [HttpGet]
        public async Task<IActionResult> AnswerTicket(int TicketId)
        {
            var model = await _ticketService.GetTicketDetail(TicketId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerTicket(TicketDetailViewModel Ticket,int TicketId)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                var model = await _ticketService.GetTicketDetail(TicketId);
                return View(model);
            }
       

            #endregion

            await _ticketService.AddMessageToCurrentTicketFromAdmin(Ticket, TicketId, User.GetCurrentUserId());
            return RedirectToAction(nameof(AllTicketList));

        }

        #endregion

        #region ClosedTicket

        public async Task<IActionResult> CloseTicket(int TicketId)
        {
            await _ticketService.CloseTicket(TicketId);
            return RedirectToAction(nameof(AllTicketList));
        }

        #endregion

    }
}
