using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Application.Tools;

using Application.DTO;
using Domain.ViewModel.AddWallet;
using Domain.ViewModel.Payment;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.UserPanel.Controllers
{
    [Authorize]
    public class PaymentController(IOrderService orderService,
        ITransactionService transactionService) : UserPanelBaseController
    {
        [HttpGet("start-pay")]
        public async Task<IActionResult> StartPay(AddWalletViewModel? wallet)
        {
            
            using HttpClient httpClient = new HttpClient();


            NovinoGetPaymentUrlRequestDto model = new NovinoGetPaymentUrlRequestDto()
            {
                CallbackMethod = "POST",
                CardPan = null,
                Email = "",
                InvoiceId = User.GetCurrentUserId().ToString(),
                MerchantId = "test",
                Mobile = null,
                Name = "",


            };
            if (wallet.Amount!=0&&wallet.UserId != 0)
            {
                model.Amount = wallet.Amount * 10;
                model.Description = "شارژ کیف پول";
                var id = await transactionService.AddNewTransaction(User.GetCurrentUserId(), wallet.Amount);
                model.TransActionId = id;
                model.CallbackUrl = $"https://localhost:7271/novinocallback/{id}";

            }
            else
            {
                var order = await orderService.GetBasketDetail(User.GetCurrentUserId());
                
                if (order == null)
                    return NotFound();
                int amount = order.Sum(od => od.FinallPrice);
                model.Amount = amount * 10;
                model.Description = "پرداخت سبد خرید";
                var id = await transactionService.AddNewTransaction(User.GetCurrentUserId(), amount);
                model.TransActionId = id;
                model.CallbackUrl = $"https://localhost:7271/novinocallback/{id}";
            }

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

        [HttpPost("novinocallback/{transId}") ,AllowAnonymous]
        public async Task<IActionResult> NovinoCallback(string paymentStatus, string invoiceID, string authority, int transId)
        {
            if (!string.IsNullOrEmpty(paymentStatus) && paymentStatus.ToLower() == "ok")
            {
                var id = int.Parse(invoiceID);
             
                using HttpClient httpClient = new HttpClient();

              
                var transactionamount = await transactionService.GetTransactionByid(transId);


                NovinoVerifyPaymentRequestDto model = new NovinoVerifyPaymentRequestDto()
                {

                    Authority = authority,
                    MerchantId = "test"
                };
                
                var order = await orderService.GetBasketDetail(id);
                if (order.Count == 0)
                {
                    model.Amount = transactionamount.Price*10;
                }
                else
                {
                    
                    int amount = order.Sum(od => od.FinallPrice);
                    if (amount == transactionamount.Price)
                    {
                        model.Amount = amount * 10;
                    }
                    else
                    {
                        model.Amount = transactionamount.Price * 10;
                    }
                }
                   
             
              
               

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
                    await orderService.CloseOrder(id, transId);

                    return RedirectToAction("SuccessPayment");
                }
                else
                {
                    await orderService.ChangeTransactionStatus(transId);
                    return RedirectToAction("UnSuccessPayment");
                }

            }
            else
            {
                return RedirectToAction("UnSuccessPayment", new ErrorPaymnetViewModel()
                {
                    Message = "خرید شما با شکست مواجه شده است. لطفا تیکت ارسال کنید.",
                    RefId = "123431"
                });
            }

        }

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
