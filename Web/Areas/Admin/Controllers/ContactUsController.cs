using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.ContactUsManagement)]
public class ContactUsController(IContactUsService service) : AdminBaseController
{
    [InvokePermission(PermissionName.ContactUsList)]
    public async Task<IActionResult> MessageList()
    {
        var messages = await service.GetAllMessagesAsync();
        return View(messages);
    }

    [InvokePermission(PermissionName.ContactUsList)]
    public async Task<IActionResult> Details(int id)
    {
        var message = await service.GetMessageByIdAsync(id);
        return View(message);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AnswerContactUs)]
    public async Task<IActionResult> Respond(int id, string adminresponse)
    {
        await service.AnswerMessageAsync(id, adminresponse);
        TempData[SuccessMessage] = "پاسخ با موفقیت ارسال شد!";
        return RedirectToAction(nameof(MessageList));
    }
}