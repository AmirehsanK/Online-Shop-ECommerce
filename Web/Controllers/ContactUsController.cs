using Application.DTO;
using Application.Services.Interfaces; 
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers
{
    [Route("contact-us")]
    public class ContactUsController : SiteBaseController 
    {
        private readonly IContactUsService _contactUsService;
        private readonly IRecaptchaVerifier _recaptchaVerifier;

        public ContactUsController(
            IContactUsService contactUsService,
            IRecaptchaVerifier recaptchaVerifier)
        {
            _contactUsService = contactUsService;
            _recaptchaVerifier = recaptchaVerifier;
        }

        #region Main page

        [HttpGet]
        public IActionResult ContactUsPage() 
        {
            return View("ContactUs"); 
        }

        #endregion

        #region ContactUs Action

        [HttpPost]
        public async Task<IActionResult> SubmitContactUs(ContactMessageDto dto)
        {
            #region Validation

            var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
            
            string? userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            var isValid = await _recaptchaVerifier.IsRecaptchaValidAsync(googleRecaptchaToken, userIpAddress);
            
            if (!isValid)
            {
                TempData["ErrorMessage"] = "کپچا را کامل کنید"; 
                return View("ContactUs", dto);
            }

            #endregion

            await _contactUsService.AddMessage(dto); 
            TempData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد";
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
