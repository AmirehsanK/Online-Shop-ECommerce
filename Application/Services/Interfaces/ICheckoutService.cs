namespace Application.Services.Interfaces;

public enum WalletCheckoutResult
{
    Paid,
    EmptyBasket,
    InsufficientBalance
}

public enum PaymentCompletionResult
{
    /// <summary>A basket payment went through and the order is closed.</summary>
    OrderPaid,

    /// <summary>A wallet top-up went through.</summary>
    WalletCharged,

    /// <summary>The basket changed after payment started, so the money went to the wallet instead.</summary>
    CreditedToWallet,

    /// <summary>This transaction was already completed; nothing was done twice.</summary>
    AlreadyCompleted,

    NotFound,
    Failed
}

public record PaymentStart(int TransactionId, int Amount);

/// <summary>
/// Everything that moves money. Amounts are always computed here from the database -
/// never taken from the browser, which used to be able to name its own price and even
/// the user whose wallet paid.
/// </summary>
public interface ICheckoutService
{
    Task<int> GetBasketTotalAsync(int userId);

    Task<int> GetWalletBalanceAsync(int userId);

    Task<WalletCheckoutResult> PayBasketFromWalletAsync(int userId);

    /// <summary>Creates a pending payment for the user's basket, or null when the basket is empty.</summary>
    Task<PaymentStart?> StartBasketPaymentAsync(int userId);

    /// <summary>Creates a pending wallet top-up, or null when the amount is not positive.</summary>
    Task<PaymentStart?> StartWalletTopUpAsync(int userId, int amount);

    /// <summary>
    /// Finishes a payment the gateway called back about. Safe to call more than once for
    /// the same transaction.
    /// </summary>
    Task<PaymentCompletionResult> CompletePaymentAsync(int transactionId, string authority);
}
