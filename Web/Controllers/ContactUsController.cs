using Application.DTO;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("contact-us")]
public class ContactUsController(IContactUsService contactUsService, IConfiguration configuration) : SiteBaseController
{
    [HttpGet]
    public IActionResult ContactUs()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ContactUs(ContactMessageDto dto)
    {
        string googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();

        //verify the token
        string secretkey = configuration["ReCaptchaSettings:SecrectKey"]!;
        string verificationUrl = configuration["ReCaptchaSettings:VerificationUrl"]!;
        bool isValid = await VerifyRecaptcha.VerifyRecaptchaV3(googleRecaptchaToken, secretkey, verificationUrl);
        if (!isValid)
        {
            TempData[ErrorMessage] = "کپچا را کامل کنید";
            return View();
        }
        await contactUsService.AddMessage(dto);
        TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
        return RedirectToAction("Index", "Home");
    }
}