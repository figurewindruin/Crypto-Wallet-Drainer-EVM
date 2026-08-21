namespace WalletDrainer.Models;

public sealed record TokenBalance
{
    public required string ContractAddress { get; init; }
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public required decimal Balance { get; init; }
    public required int Decimals { get; init; }
    public decimal PriceUsd { get; init; }
    public decimal ValueUsd => Balance * PriceUsd;
    public bool IsVerified { get; init; }
}
