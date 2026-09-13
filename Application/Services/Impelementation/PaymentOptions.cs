namespace Application.Services.Impelementation;

/// <summary>Bound from the "Payment" configuration section.</summary>
public class PaymentOptions
{
    public const string SectionName = "Payment";

    /// <summary>"Novino" for the real gateway, "Demo" for the built-in local stand-in.</summary>
    public string Provider { get; set; } = "Novino";

    public string MerchantId { get; set; } = "test";

    public string NovinoBaseUrl { get; set; } = "https://api.novinopay.com/payment/ipg/v2/";
}
