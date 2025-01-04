using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using MyEshop.Data.DTOs.Novino;
using MyEshop.Data.ViewModels.Payment;

namespace Web.Areas.UserPanel.Controllers
{
    public class PaymentController : UserPanelBaseController
    {
        public async Task<IActionResult> StartPay(int orderId)
        {
            var order = await _context.Orders
                .Include(od => od.OrderDetails)
                .Include(od => od.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            using HttpClient httpClient = new HttpClient();

            int amount = order.OrderDetails.Sum(od => od.Price * od.Count);

            NovinoGetPaymentUrlRequestDto model = new NovinoGetPaymentUrlRequestDto()
            {
                Amount = amount * 10,
                CallbackMethod = "POST",
                CallbackUrl = "https://localhost:7094/Payment/NovinoCallback",
                CardPan = null,
                Description = "پرداخت سبد خرید",
                Email = order.User.Email,
                InvoiceId = order.Id.ToString(),
                MerchantId = "test",
                Mobile = null,
                Name = order.User.UserName
            };

            string body = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                      "https://api.novinopay.com/payment/ipg/v2/request",
                      content
                      );

            string responseContent = await response.Content.ReadAsStringAsync();

            var finalResult = JsonConvert.DeserializeObject<NovinoGetPaymentUrlResponseDto>(responseContent);

            if (finalResult != null && finalResult.Status == "100")
            {
                return Redirect(finalResult.Data.PaymentUrl);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> NovinoCallback(string paymentStatus, string invoiceID, string authority)
        {
            if (!string.IsNullOrEmpty(paymentStatus) && paymentStatus.ToLower() == "ok")
            {
                var order = await _context.Orders
                .Include(od => od.OrderDetails)
                .Include(od => od.User)
                .FirstOrDefaultAsync(o => o.Id == int.Parse(invoiceID));

                if (order == null)
                    return NotFound();

                using HttpClient httpClient = new HttpClient();

                int amount = order.OrderDetails.Sum(od => od.Price * od.Count);

                NovinoVerifyPaymentRequestDto model = new NovinoVerifyPaymentRequestDto()
                {
                    Amount = amount * 10,
                    Authority = authority,
                    MerchantId = "test"
                };

                string body = JsonConvert.SerializeObject(model);

                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(
                          "https://api.novinopay.com/payment/ipg/v2/verification",
                          content
                          );

                string responseContent = await response.Content.ReadAsStringAsync();

                var finalResult = JsonConvert.DeserializeObject<NovinoVerifyPaymentResponseDto>(responseContent);

                if (finalResult != null && finalResult.Status == "100")
                {
                    order.IsFinaly = true;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    return View("SuccessPayment", new SuccessPaymnetViewModel()
                    {
                        Message = "پرداخت با موفقیت انجام شد.",
                        RefId = finalResult.Data.RefId
                    });
                }
                else
                {
                    return View("ErrorPayment", new ErrorPaymnetViewModel()
                    {
                        Message = "خرید شما با شکست مواجه شده است. لطفا تیکت ارسال کنید.",
                        RefId = "123431"
                    });
                }

            }
            else
            {
                return View("ErrorPayment", new ErrorPaymnetViewModel()
                {
                    Message = "خرید شما با شکست مواجه شده است. لطفا تیکت ارسال کنید.",
                    RefId = "123431"
                });
            }

        }

    }
}
