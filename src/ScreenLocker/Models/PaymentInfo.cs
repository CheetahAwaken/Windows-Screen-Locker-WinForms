namespace ScreenLocker.Models;

public sealed record PaymentInfo
{
    public required string WalletAddress { get; init; }
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }
    public string? TransactionId { get; init; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }

    public string GetPaymentUri()
    {
        return $"bitcoin:{WalletAddress}?amount={Amount}&label=Unlock";
    }
}
