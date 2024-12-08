using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class ContactUsController(IContactUsService service) : AdminBaseController
{
    [HttpGet]
    public async Task<IActionResult> MessageList()
    {
        var messages = await service.GetAllMessagesAsync();
        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var message = await service.GetMessageByIdAsync(id);
        return View(message);
    }

    [HttpPost]
    public async Task<IActionResult> Respond(int id, string adminresponse)
    {
        await service.AnswerMessageAsync(id, adminresponse);
        TempData[SuccessMessage] = "پاسخ با موفقیت ارسال شد!";
        return RedirectToAction(nameof(MessageList));
    }
}
