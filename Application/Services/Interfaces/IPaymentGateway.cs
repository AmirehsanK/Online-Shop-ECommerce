namespace Application.Services.Interfaces;

/// <summary>
/// A card-payment provider. Amounts are in toman, the unit the rest of the app uses.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>Returns the URL to send the customer to, or null if the provider refused.</summary>
    Task<string?> RequestPaymentAsync(int transactionId, int amount, string description, string callbackUrl);

    /// <summary>Asks the provider whether the payment identified by <paramref name="authority"/> really went through for this amount.</summary>
    Task<bool> VerifyPaymentAsync(string authority, int amount);
}
