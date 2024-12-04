using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

public class TicketController : UserPanelBaseController
{
    [HttpGet]
    public async Task<IActionResult> TicketList()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddTicket()
    {
        return View();
    }
}