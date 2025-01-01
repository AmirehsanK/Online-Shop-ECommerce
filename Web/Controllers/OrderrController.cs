using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace Web.Controllers
{
    public class OrderrController(IUserService userService) : SiteBaseController
    {
        public async Task<IActionResult> Payment()
        {
            var user = await userService.GetUserById(User.GetCurrentUserId());
            var pay = new Payment(1000);
            var result =await pay.PaymentRequest("پرداخت فاکتور", "https://localhost:7271/onlinepayment/"+"123",user.Email,user.PhoneNumber);
            if (result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
            }
            else
            {
                return BadRequest();
            }
            return null;
        }

        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"]!=""&&
                HttpContext.Request.Query["Status"].ToString().ToLower()=="ok"&&
                 HttpContext.Request.Query["Authority"]!="")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = 12;//order id
                var payment = new Payment(1000);//order sum
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    //order is done
                    ViewBag.code = res.RefId;
                    return View();
                }
            }
            return NotFound();
        }
    }
}
