using WalletDrainer.Models;

namespace WalletDrainer.Core;

public sealed class AssetCollector
{
    public List<WalletAsset> PrioritizeAssets(List<WalletAsset> assets)
    {
        return assets
            .OrderByDescending(a => a.UsdValue)
            .ThenBy(a => a.IsNative ? 1 : 0)
            .ToList();
    }

    public decimal TotalValueUsd(IEnumerable<WalletAsset> assets) =>
        assets.Sum(a => a.UsdValue);

    public Dictionary<string, decimal> ValueByChain(IEnumerable<WalletAsset> assets) =>
        assets.GroupBy(a => a.Chain)
              .ToDictionary(g => g.Key, g => g.Sum(a => a.UsdValue));
}
