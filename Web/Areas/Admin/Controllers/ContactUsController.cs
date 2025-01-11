using Application.Services.Interfaces;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.ContactUsManagement)]
public class ContactUsController(IContactUsService service) : AdminBaseController
{
    #region Message List

    [InvokePermission(PermissionName.ContactUsList)]
    public async Task<IActionResult> MessageList()
    {
        var messages = await service.GetAllMessagesAsync();
        return View(messages);
    }

    #endregion

    #region Message Details

    [InvokePermission(PermissionName.ContactUsList)]
    public async Task<IActionResult> Details(int id)
    {
        var message = await service.GetMessageByIdAsync(id);
        return View(message);
    }

    #endregion

    #region Message Respond

    [HttpPost]
    [InvokePermission(PermissionName.AnswerContactUs)]
    public async Task<IActionResult> Respond(int id, string adminResponse)
    {
        await service.AnswerMessageAsync(id, adminResponse);
        TempData[SuccessMessage] = "پاسخ با موفقیت ارسال شد!";
        return RedirectToAction(nameof(MessageList));
    }

    #endregion
    
}