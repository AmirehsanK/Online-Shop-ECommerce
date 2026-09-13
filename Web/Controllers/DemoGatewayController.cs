using System.Globalization;
using System.Text.Encodings.Web;
using Application.Services.Impelementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Web.Controllers;

/// <summary>
/// The "bank page" for <see cref="DemoPaymentGateway"/>: two buttons that post the same form a
/// real gateway would post to the callback. Returns 404 unless Payment:Provider is "Demo".
/// </summary>
public class DemoGatewayController(IOptions<PaymentOptions> options) : Controller
{
    [HttpGet("demo-gateway")]
    public IActionResult Index(int amount, string callback, string authority)
    {
        if (!string.Equals(options.Value.Provider, "Demo", StringComparison.OrdinalIgnoreCase)
            || !Uri.TryCreate(callback, UriKind.RelativeOrAbsolute, out var callbackUri))
            return NotFound();

        // Post back to this app only, whatever host the callback URL was built with.
        var callbackPath = callbackUri.IsAbsoluteUri ? callbackUri.PathAndQuery : callback;
        if (!Url.IsLocalUrl(callbackPath))
            return NotFound();

        var action = HtmlEncoder.Default.Encode(callbackPath);
        var auth = HtmlEncoder.Default.Encode(authority);
        var price = amount.ToString("#,0", CultureInfo.InvariantCulture);

        var html = $$"""
            <!doctype html>
            <html lang="fa" dir="rtl"><head><meta charset="utf-8"><title>درگاه پرداخت آزمایشی</title>
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <style>
              body{font-family:system-ui,Tahoma,sans-serif;background:#f3f4f6;display:grid;place-items:center;min-height:100vh;margin:0}
              .card{background:#fff;border-radius:12px;padding:32px;box-shadow:0 4px 24px #0001;text-align:center;min-width:300px}
              .amount{font-size:28px;font-weight:700;margin:16px 0 24px}
              button{border:0;border-radius:8px;padding:12px 24px;font-size:16px;cursor:pointer;margin:4px}
              .pay{background:#16a34a;color:#fff}.cancel{background:#e5e7eb}
              small{display:block;color:#6b7280;margin-top:16px}
            </style></head>
            <body><div class="card">
              <div>درگاه پرداخت آزمایشی</div>
              <div class="amount">{{price}} تومان</div>
              <form method="post" action="{{action}}" style="display:inline">
                <input type="hidden" name="authority" value="{{auth}}"><input type="hidden" name="paymentStatus" value="OK">
                <button class="pay">پرداخت</button></form>
              <form method="post" action="{{action}}" style="display:inline">
                <input type="hidden" name="authority" value="{{auth}}"><input type="hidden" name="paymentStatus" value="NOK">
                <button class="cancel">انصراف</button></form>
              <small>Demo gateway: no real money moves.</small>
            </div></body></html>
            """;
        return Content(html, "text/html; charset=utf-8");
    }
}
