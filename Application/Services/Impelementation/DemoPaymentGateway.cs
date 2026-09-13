using Application.Services.Interfaces;

namespace Application.Services.Impelementation;

/// <summary>
/// Stands in for a real provider so checkout can be run end to end locally and in the
/// Docker demo. It sends the customer to a page inside this app that posts back to the
/// normal callback, so everything after the redirect is the production code path.
/// Only registered when Payment:Provider is "Demo".
/// </summary>
public class DemoPaymentGateway : IPaymentGateway
{
    public const string AuthorityPrefix = "demo-";

    public Task<string?> RequestPaymentAsync(int transactionId, int amount, string description, string callbackUrl)
    {
        var url = $"/demo-gateway?amount={amount}&callback={Uri.EscapeDataString(callbackUrl)}" +
                  $"&authority={AuthorityPrefix}{transactionId}";
        return Task.FromResult<string?>(url);
    }

    public Task<bool> VerifyPaymentAsync(string authority, int amount) =>
        Task.FromResult(authority.StartsWith(AuthorityPrefix, StringComparison.Ordinal));
}
