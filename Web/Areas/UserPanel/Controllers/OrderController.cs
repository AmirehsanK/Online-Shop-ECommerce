using System.Text;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Interface;
using Domain.ViewModel;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Domain.Enums;
using Domain.ViewModel.AddWallet;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.UserPanel.Controllers
{
    public class OrderController
        (IOrderService orderService,IUserService userService,IConfiguration configuration,
            ITransactionService transactionService): UserPanelBaseController
    {


        [HttpGet("BasketDetail")]
        public async Task<IActionResult> BasketDetail()
        {
            var model = await orderService.GetBasketDetail(User.GetCurrentUserId());
            return View(model);
        }

      

        #region AddToBasket

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int? productColorId)
        {
            if (User.Identity.IsAuthenticated)
            {

               var res= await orderService.AddProductToOrder(productId, User.GetCurrentUserId(),productColorId);
               switch (res)
               {
                   case AddToBasketResult.Success:
                       if (productColorId!=null)
                       {
                           await orderService.MinuesColorCount(productColorId.Value);
                       }
                       TempData[SuccessMessage] = "محصول به سبد خرید اضافه شد";
                       return Redirect(HttpContext.Response.Headers.Referer);
                       
                   case AddToBasketResult.Failed:
                       TempData[ErrorMessage] = "با خطا مواجه شد";
                       return Redirect(HttpContext.Response.Headers.Referer);
             
               }

            }

            return RedirectToAction("login", "UserAuthentication");

        }

        #endregion

        #region ChooseAddrress

        [Route("ChooseAddress")]
        [HttpGet]
        public async Task<IActionResult> ChooseAddress()
        {
            var model = await orderService.GetUserAddressForOrder(User.GetCurrentUserId());
            ViewData["BasketDetail"] = await orderService.GetBasketDetail(User.GetCurrentUserId());
            return View(model);
        }
        [Route("ChooseAddress")]
        [HttpPost]
        public async Task<IActionResult> ChooseAddress(GetUserAddressForOrderViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await orderService.AddUserAddressForOrder(model, User.GetCurrentUserId());
            var models = await orderService.GetUserAddressForOrder(User.GetCurrentUserId());
            return View(models);
        }

        #endregion

        public async Task<IActionResult> ChoosePaymentWay()
        {
            ViewData["BasketDetail"] = await orderService.GetBasketDetail(User.GetCurrentUserId());
            return View();
        }
        public async Task<IActionResult> CheckWalletBalance(int amount, int UserId)
        {
            var res = await transactionService.GetUserBalanceWallet(UserId, amount);
            switch (res)
            {
                case WalletStatusBalance.IsOkay:
                    
                    return RedirectToAction("SuccessPayment", "Payment");
                    
                case WalletStatusBalance.NoneBalance:
                    var wallet = new AddWalletViewModel
                    {
                        Amount = amount,
                        UserId = UserId
                    };
                    return RedirectToAction("StartPay", "Payment", wallet);
                    break;
             
            }
                    
            
            return View();
        }
    }
}
