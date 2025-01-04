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
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.UserPanel.Controllers
{
    public class OrderController
        (IOrderService orderService,IUserService userService,IConfiguration configuration): UserPanelBaseController
    {
        string merchant = "cfa83c81-89b0-4993-9445-2c3fcd323455";
        string amount = "1100";
        string authority;
        string description = "خرید تستی ";
        string callbackurl = "https://localhost:7271/user/VerifyByHttpClient";
        #region BasketDetail

        [HttpGet("BasketDetail")]
        public async Task<IActionResult> BasketDetail()
        {
            var model = await orderService.GetBasketDetail(User.GetCurrentUserId());
            return View(model);
        }

        #endregion

        #region AddToBasket

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int productColorId)
        {
            if (User.Identity.IsAuthenticated)
            {
                await orderService.AddProductToOrder(productId, User.GetCurrentUserId(),productColorId);
                await orderService.MinuesColorCount(productColorId);
                return Json(new { success = true, message = "محصول به سبد خرید اضافه شد." });
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



    }
}
