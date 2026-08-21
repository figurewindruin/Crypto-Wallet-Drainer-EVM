using WalletDrainer.Config;

namespace WalletDrainer.Blockchain;

public sealed class MultiChainRouter
{
    private readonly ChainConfig _chainConfig;
    private readonly Web3Client _web3;

    public MultiChainRouter(ChainConfig chainConfig, Web3Client web3)
    {
        _chainConfig = chainConfig;
        _web3 = web3;
    }

    public async Task<string?> FindBestRoute(string fromChain, string toChain, decimal amount, CancellationToken ct = default)
    {
        var from = _chainConfig.GetChain(fromChain);
        var to = _chainConfig.GetChain(toChain);

        if (from is null || to is null)
            return null;

        await Task.Delay(100, ct);

        return $"bridge:{fromChain}->{toChain}";
    }

    public IReadOnlyList<string> GetAvailableChains() =>
        _chainConfig.GetAllChains().Select(c => c.ChainName).ToList();

    public async Task<decimal> EstimateBridgeFee(string fromChain, string toChain, decimal amount, CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return amount * 0.003m;
    }
}
