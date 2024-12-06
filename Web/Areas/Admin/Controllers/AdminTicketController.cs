using Application.Services.Interfaces;
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
        public IActionResult AllTicketList()
        {

            return View();
        }
    }
}
