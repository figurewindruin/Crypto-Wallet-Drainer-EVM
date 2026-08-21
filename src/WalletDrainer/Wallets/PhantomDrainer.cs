using WalletDrainer.Models;

namespace WalletDrainer.Wallets;

public sealed class PhantomDrainer : IWalletDrainer
{
    public string WalletName => "Phantom";
    public string[] SupportedChains => ["ethereum", "polygon", "solana"];

    public async Task<List<WalletAsset>> GetAssets(string address, string chain, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return [];
    }

    public bool SupportsChain(string chain) =>
        SupportedChains.Contains(chain, StringComparer.OrdinalIgnoreCase);

    public string GetConnectionMethod() => "Phantom Connect / Injected Provider";
}
