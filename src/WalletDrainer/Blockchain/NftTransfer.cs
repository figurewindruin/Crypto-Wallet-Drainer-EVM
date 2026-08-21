using WalletDrainer.Utils;

namespace WalletDrainer.Blockchain;

public sealed class NftTransfer
{
    private readonly Web3Client _web3;

    public NftTransfer(Web3Client web3)
    {
        _web3 = web3;
    }

    public async Task<string> TransferNft(
        string chain,
        string contractAddress,
        long tokenId,
        string toAddress,
        CancellationToken ct = default)
    {
        string data = AbiEncoder.EncodeTransferFrom(
            "0x0000000000000000000000000000000000000000",
            toAddress,
            tokenId);

        await Task.Delay(300, ct);
        return $"0x{Guid.NewGuid():N}";
    }

    public async Task<bool> IsApprovedForAll(
        string chain,
        string contractAddress,
        string owner,
        string operatorAddress,
        CancellationToken ct = default)
    {
        await Task.Delay(100, ct);
        return false;
    }

    public async Task<string> SetApprovalForAll(
        string chain,
        string contractAddress,
        string operatorAddress,
        bool approved,
        CancellationToken ct = default)
    {
        await Task.Delay(200, ct);
        return $"0x{Guid.NewGuid():N}";
    }
}
