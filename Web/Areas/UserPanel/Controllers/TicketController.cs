using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Ticket;
using Domain.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

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
            var model = await _ticketService.GetTicketListForShow(User.GetCurrentUserId());
            return View(model);
        }
        return View();
    }

    #endregion

    #region َAddTicket

    [HttpGet]
    public async Task<IActionResult> AddTicket()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddTicket(AddTicketViewModel model)
    {
        #region Validation

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        #endregion

        await _ticketService.AddNewTicket(model,User.GetCurrentUserId());
        return View();
    }

    #endregion

    #region TicketDetail

    [HttpGet("TicketDetail/{ticketid}")]
    public async Task<IActionResult> TicketDetail(int ticketid)
    {
        var model=await _ticketService.GetTicketDetail(ticketid);
        return View(model);
    }
    [HttpPost("TicketDetail/{ticketid}")]
    public async Task<IActionResult> TicketDetail(TicketDetailViewModel model, int ticketid)
    {
        #region Validation

        if (!ModelState.IsValid)
        {
            var models = await _ticketService.GetTicketDetail(ticketid);
            return View(models);
        }

        #endregion


        await _ticketService.AddMessageToCurrentTicket(model, ticketid, User.GetCurrentUserId());
        return RedirectToAction(nameof(TicketList));
    }

    #endregion



}


