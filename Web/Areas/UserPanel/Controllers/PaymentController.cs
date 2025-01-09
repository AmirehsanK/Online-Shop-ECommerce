using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Application.Tools;

using Application.DTO;
using Domain.ViewModel.Payment;

namespace Web.Areas.UserPanel.Controllers
{
    public class PaymentController(IOrderService orderService) : UserPanelBaseController
    {
        #region Start Pay

        [HttpGet("start-pay")]
        public async Task<IActionResult> StartPay()
        {
            var order = await orderService.GetBasketDetail(User.GetCurrentUserId());

            if (order == null!)
                return NotFound();

            using var httpClient = new HttpClient();
            var amount = order.Sum(od => od.FinallPrice * od.ProductCount);
            var model = new NovinoGetPaymentUrlRequestDto()
            {
                Amount = amount * 10,
                CallbackMethod = "POST",
                CallbackUrl = "https://localhost:7271/novinocallback",
                CardPan = null!,
                Description = "پرداخت سبد خرید",
                Email = "",
                InvoiceId = User.GetCurrentUserId().ToString(),
                MerchantId = "test",
                Mobile = null!,
                Name = ""
            };
            var body = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(
                      "https://api.novinopay.com/payment/ipg/v2/request",
                      content
                      );

            var responseContent = await response.Content.ReadAsStringAsync();
            var finalResult = JsonConvert.DeserializeObject<NovinoGetPaymentUrlResponseDto>(responseContent);
            if (finalResult is { Status: "100" })
            {
                return Redirect(finalResult.Data.PaymentUrl);
            }
            else
            {
                return NotFound();
            }
        }
        
        #endregion
        
        #region Novino Call back
        
        [HttpPost("novinocallback")]
        public async Task<IActionResult> NovinoCallback(string paymentStatus, string invoiceId, string authority)
        {
            if (!string.IsNullOrEmpty(paymentStatus) && paymentStatus.ToLower() == "ok")
            {
                var id=int.Parse(invoiceId);
                var order = await orderService.GetBasketDetail(id);
                
                if (order == null!)
                    return NotFound();

                using var httpClient = new HttpClient();

                var amount = order.Sum(od => od.FinallPrice * od.ProductCount);
                var model = new NovinoVerifyPaymentRequestDto()
                {
                    Amount = amount * 10,
                    Authority = authority,
                    MerchantId = "test"
                };
                var body = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(
                          "https://api.novinopay.com/payment/ipg/v2/verification",
                          content
                          );

                var responseContent = await response.Content.ReadAsStringAsync();
                var finalResult = JsonConvert.DeserializeObject<NovinoVerifyPaymentResponseDto>(responseContent);
                if (finalResult is { Status: "100" })
                {
                    await orderService.CloseOrder(id);
                    return View("SuccessPayment");
                }
                else
                {
                    return View("UnSuccessPayment");
                }
            }
            else
            {
                return View("UnSuccessPayment", new ErrorPaymnetViewModel()
                {
                    Message = "خرید شما با شکست مواجه شده است. لطفا تیکت ارسال کنید.",
                    RefId = "123431"
                });
            }

        }
        
        #endregion
        
        #region SuccessPayment
        
        [HttpGet("SuccessPayment")]
        public async Task<IActionResult> SuccessPayment()
        {
            return View();
        }

        #endregion

        #region UnSuccessPayment

        public async Task<IActionResult> UnSuccessPayment()
        {
            return View();
        }

        #endregion
    }
}
