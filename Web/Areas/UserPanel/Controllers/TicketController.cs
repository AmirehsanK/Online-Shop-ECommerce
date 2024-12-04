using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers
{

    public class TicketController : UserPanelBaseController
    {
        #region Ctor

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region TicketList

        [HttpGet]
        public async Task<IActionResult> TicketList()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model= await _ticketService.GetTicketListForShow(User.GetCurrentUserId());
                return View(model);
            }
            return View();
        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTicket(AddTicketViewModel model,int ticket)
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> TicketDetail()
        {
            return View();
        }
    }
}
