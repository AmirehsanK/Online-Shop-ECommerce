using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.AddWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Authorize]
public class PaymentController(
    ICheckoutService checkoutService,
    IPaymentGateway paymentGateway,
    IConfiguration configuration) : UserPanelBaseController
{
    #region Start Pay

    /// <summary>Card payment for the whole basket. The amount comes from the basket, not the request.</summary>
    [HttpGet("start-pay")]
    public async Task<IActionResult> StartPay()
    {
        var payment = await checkoutService.StartBasketPaymentAsync(User.GetCurrentUserId());
        if (payment == null)
        {
            TempData[ErrorMessage] = "سبد خرید شما خالی است";
            return RedirectToAction("BasketDetail", "Order");
        }

        return await RedirectToGateway(payment, "پرداخت سبد خرید");
    }

    /// <summary>Wallet top-up. The amount is the customer's choice; the user is always the signed-in one.</summary>
    [HttpGet("top-up-wallet")]
    public async Task<IActionResult> TopUpWallet(AddWalletViewModel wallet)
    {
        var payment = await checkoutService.StartWalletTopUpAsync(User.GetCurrentUserId(), wallet.Amount);
        if (payment == null)
        {
            TempData[ErrorMessage] = "مبلغ وارد شده معتبر نیست";
            return RedirectToAction("AddTransactionToWallet", "Home");
        }

        return await RedirectToGateway(payment, "شارژ کیف پول");
    }

    private async Task<IActionResult> RedirectToGateway(PaymentStart payment, string description)
    {
        var callbackUrl = $"{configuration["ApplicationSettings:DomainLink"]!.TrimEnd('/')}/novinocallback/{payment.TransactionId}";
        var paymentUrl = await paymentGateway.RequestPaymentAsync(payment.TransactionId, payment.Amount, description, callbackUrl);
        if (paymentUrl == null)
            return RedirectToAction(nameof(UnSuccessPayment));

        return Redirect(paymentUrl);
    }

    #endregion

    #region Novino Call back

    /// <summary>
    /// Where the gateway sends the customer back. It is anonymous because the gateway posts it,
    /// so nothing in the request is trusted: the transaction id only selects a payment we
    /// created, and the gateway is asked to confirm it for the amount we recorded. The user,
    /// the amount and whether it pays a basket all come from that stored transaction.
    /// </summary>
    [HttpPost("novinocallback/{transId:int}")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> NovinoCallback(int transId, string paymentStatus, string authority)
    {
        if (!string.Equals(paymentStatus, "ok", StringComparison.OrdinalIgnoreCase))
            return RedirectToAction(nameof(UnSuccessPayment));

        return await checkoutService.CompletePaymentAsync(transId, authority) switch
        {
            PaymentCompletionResult.OrderPaid or PaymentCompletionResult.AlreadyCompleted
                or PaymentCompletionResult.WalletCharged or PaymentCompletionResult.CreditedToWallet
                => RedirectToAction(nameof(SuccessPayment)),
            _ => RedirectToAction(nameof(UnSuccessPayment))
        };
    }

    #endregion

    #region SuccessPayment

    [HttpGet("SuccessPayment")]
    [AllowAnonymous]
    public IActionResult SuccessPayment()
    {
        return View();
    }

    #endregion

    #region UnSuccessPayment

    [AllowAnonymous]
    public IActionResult UnSuccessPayment()
    {
        return View();
    }

    #endregion
}
