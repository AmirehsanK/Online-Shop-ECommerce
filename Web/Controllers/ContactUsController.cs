using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ContactUsController : Controller
{
    [HttpGet("contact-us")]
    public IActionResult ContactUs()
    {
        return View();
    }
}