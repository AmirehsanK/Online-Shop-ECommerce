using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

public class TicketController(ITicketService ticketService) : UserPanelBaseController
{
    #region TicketList

    [HttpGet]
    public async Task<IActionResult> TicketList()
    {
        if (!User.Identity!.IsAuthenticated) return View();
        var model = await ticketService.GetTicketListForShow(User.GetCurrentUserId());
        return View(model);
    }

    #endregion

    #region َAddTicket

    [HttpGet]
    public IActionResult AddTicket()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket(AddTicketViewModel model)
    {
        #region Validation

        if (!ModelState.IsValid) return View(model);

        #endregion

        await ticketService.AddNewTicket(model, User.GetCurrentUserId());
        return RedirectToAction(nameof(TicketList));
    }

    #endregion

    #region TicketDetail

    [HttpGet("TicketDetail/{ticketId:int}")]
    public async Task<IActionResult> TicketDetail(int ticketId)
    {
        var model = await ticketService.GetTicketDetail(ticketId);
        return View(model);
    }

    [HttpPost("TicketDetail/{ticketId:int}")]
    public async Task<IActionResult> TicketDetail(TicketDetailViewModel model, int ticketId)
    {
        #region Validation

        if (!ModelState.IsValid)
        {
            var models = await ticketService.GetTicketDetail(ticketId);
            return View(models);
        }

        #endregion

        await ticketService.AddMessageToCurrentTicket(model, ticketId, User.GetCurrentUserId());
        return RedirectToAction(nameof(TicketList));
    }

    #endregion
}