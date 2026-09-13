using System.Text;
using Application.DTO;
using Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Services.Impelementation;

public class NovinoPaymentGateway(HttpClient httpClient, IOptions<PaymentOptions> options) : IPaymentGateway
{
    // Novino takes rials; the app works in toman.
    private const int RialsPerToman = 10;
    private const string SuccessStatus = "100";

    private readonly PaymentOptions _options = options.Value;

    public async Task<string?> RequestPaymentAsync(int transactionId, int amount, string description, string callbackUrl)
    {
        var request = new NovinoGetPaymentUrlRequestDto
        {
            MerchantId = _options.MerchantId,
            Amount = amount * RialsPerToman,
            CallbackUrl = callbackUrl,
            CallbackMethod = "POST",
            InvoiceId = transactionId.ToString(),
            Description = description,
            Email = "",
            Name = "",
            Mobile = null!,
            CardPan = null!
        };

        var response = await PostAsync<NovinoGetPaymentUrlResponseDto>("request", request);
        return response is { Status: SuccessStatus } ? response.Data.PaymentUrl : null;
    }

    public async Task<bool> VerifyPaymentAsync(string authority, int amount)
    {
        var request = new NovinoVerifyPaymentRequestDto
        {
            MerchantId = _options.MerchantId,
            Authority = authority,
            Amount = amount * RialsPerToman
        };

        var response = await PostAsync<NovinoVerifyPaymentResponseDto>("verification", request);
        return response is { Status: SuccessStatus };
    }

    private async Task<T?> PostAsync<T>(string path, object body)
    {
        using var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(new Uri(new Uri(_options.NovinoBaseUrl), path), content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }
}
