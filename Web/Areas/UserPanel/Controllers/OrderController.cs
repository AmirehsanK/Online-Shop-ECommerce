using System.Text;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Web.Areas.UserPanel.Controllers
{
    public class OrderController
        (IOrderService orderService,IUserService userService,IConfiguration configuration): UserPanelBaseController
    {
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

        public async Task<IActionResult> ChooseAddress()
        {
            return View();
        }

        #endregion

    }
}
