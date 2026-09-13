using Application.Services.Interfaces;
using Application.Tools;
using Domain.Enums;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Authorize]
public class OrderController(
    IOrderService orderService,
    ICheckoutService checkoutService) : UserPanelBaseController
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
        var res = await orderService.AddProductToOrder(productId, User.GetCurrentUserId(), productColorId);
        switch (res)
        {
            case AddToBasketResult.Success:
                TempData[SuccessMessage] = "محصول به سبد خرید اضافه شد";
                break;
            case AddToBasketResult.OutOfStock:
                TempData[ErrorMessage] = "موجودی این محصول به پایان رسیده است";
                break;
            default:
                TempData[ErrorMessage] = "با خطا مواجه شد";
                break;
        }

        return RedirectToReferer();
    }

    #endregion

    public async Task<IActionResult> ChoosePaymentWay()
    {
        ViewData["BasketDetail"] = await orderService.GetBasketDetail(User.GetCurrentUserId());
        return View();
    }

    #region Pay From Wallet

    /// <summary>
    /// Pays the signed-in user's basket from their wallet. This used to be a GET that took
    /// the amount and the user id from the query string, so anyone could pay any basket for
    /// one toman, or spend someone else's balance.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayWithWallet()
    {
        switch (await checkoutService.PayBasketFromWalletAsync(User.GetCurrentUserId()))
        {
            case WalletCheckoutResult.Paid:
                return RedirectToAction("SuccessPayment", "Payment");
            case WalletCheckoutResult.InsufficientBalance:
                TempData[ErrorMessage] = "موجودی کیف پول کافی نیست";
                return RedirectToAction(nameof(ChoosePaymentWay));
            default:
                TempData[ErrorMessage] = "سبد خرید شما خالی است";
                return RedirectToAction(nameof(BasketDetail));
        }
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
        if (!ModelState.IsValid) return View(model);

        await orderService.AddUserAddressForOrder(model, User.GetCurrentUserId());
        var models = await orderService.GetUserAddressForOrder(User.GetCurrentUserId());
        return View(models);
    }

    #endregion

    private IActionResult RedirectToReferer()
    {
        var referer = Request.Headers.Referer.ToString();
        return Url.IsLocalUrl(referer) || Uri.TryCreate(referer, UriKind.Absolute, out var uri) && uri.Host == Request.Host.Host
            ? Redirect(referer)
            : RedirectToAction(nameof(BasketDetail));
    }
}
