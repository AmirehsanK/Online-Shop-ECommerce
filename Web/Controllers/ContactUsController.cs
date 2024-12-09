using Application.DTO;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("contact-us")]
public class ContactUsController(IContactUsService contactUsService) : SiteBaseController
{
    [HttpGet]
    public IActionResult ContactUs()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ContactUs(ContactMessageDto dto)
    {
        await contactUsService.AddMessage(dto);
        TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
        return RedirectToAction("Index", "Home");
    }
}