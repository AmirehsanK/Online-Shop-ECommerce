using Application.DTO;
using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("contact-us")]
public class ContactUsController(IContactUsService contactUsService, IConfiguration configuration) : SiteBaseController
{
    #region Main page

    [HttpGet]
    public IActionResult ContactUs()
    {
        return View();
    }
    
    #endregion
    
    #region ContactUs

    [HttpPost]
    public async Task<IActionResult> ContactUs(ContactMessageDto dto)
    {
        #region Validation

        var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        var secretKey = configuration["ReCaptchaSettings:SecretKey"]!;
        var verificationUrl = configuration["ReCaptchaSettings:VerificationUrl"]!;
        var isValid = await VerifyRecaptcha.VerifyRecaptchaV3(googleRecaptchaToken, secretKey, verificationUrl);
        if (!isValid)
        {
            TempData[ErrorMessage] = "کپچا را کامل کنید";
            return View();
        }
        
        #endregion
        await contactUsService.AddMessage(dto);
        TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
        return RedirectToAction("Index", "Home");
    }
    
    #endregion
}