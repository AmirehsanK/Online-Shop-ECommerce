using Newtonsoft.Json;

namespace Application.DTO;

public class NovinoVerifyPaymentResponseDto
{
    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("message")] public string Message { get; set; }

    [JsonProperty("data")] public NovinoVerifyPaymentResponseData Data { get; set; }

    [JsonProperty("errors")] public object Errors { get; set; }
}

public class NovinoVerifyPaymentResponseData
{
    [JsonProperty("ref_id")] public string RefId { get; set; }

    [JsonProperty("card_pan")] public string CardPan { get; set; }

    [JsonProperty("amount")] public int Amount { get; set; }

    [JsonProperty("invoice_id")] public string InvoiceId { get; set; }

    [JsonProperty("buyer_ip")] public string BuyerIp { get; set; }

    [JsonProperty("payment_time")] public long PaymentTime { get; set; }

    [JsonProperty("trans_id")] public int TransactionId { get; set; }
}