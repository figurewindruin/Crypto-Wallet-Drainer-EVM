namespace WalletDrainer.Models;

public sealed class WalletAsset
{
    public required string Chain { get; init; }
    public string? ContractAddress { get; init; }
    public string Symbol { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public decimal UsdValue { get; init; }
    public bool IsNative { get; init; }
    public bool IsNft { get; init; }
    public long? TokenId { get; init; }
    public int Decimals { get; init; } = 18;
    public string? Name { get; init; }
}
