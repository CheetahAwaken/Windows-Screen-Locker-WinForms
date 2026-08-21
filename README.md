# ScreenLocker

[![Build Status](https://img.shields.io/github/actions/workflow/status/maldev-research/ScreenLocker/build.yml?branch=main)](https://github.com/maldev-research/ScreenLocker/actions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Stars](https://img.shields.io/github/stars/maldev-research/ScreenLocker?style=social)](https://github.com/maldev-research/ScreenLocker)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)]()

> **Windows Screen Locker | Fullscreen Lock + Input Block + Timer | WinForms**

A proof-of-concept screen locker application implementing fullscreen topmost forms, keyboard/mouse input blocking, task manager disabling, process kill guards, and countdown timers. Built with WinForms for Windows desktop environments.

---

## Features

- **Fullscreen Lock Screen**
  - Topmost fullscreen form covering all monitors
  - Custom ransom-style message display
  - Countdown timer with deadline
  - QR code placeholder for wallet address
  - Unlock code dialog with attempt limiting

- **Input Protection**
  - Low-level keyboard hook (blocks Win key, Alt+Tab)
  - Task Manager disabling via registry
  - Alt+Tab and Ctrl+Alt+Del blocking via hotkey registration
  - Process kill guard (restarts self if terminated)
  - Dangerous process termination (cmd, powershell, regedit, taskmgr)

- **Persistence**
  - Registry Run key auto-start
  - Shell replacement option (replaces explorer.exe)
  - Safe boot prevention (F8 menu disabling)
  - All-users registration support

- **Configuration**
  - JSON-based lock and message configuration
  - Customizable unlock code
  - Configurable lock duration and deadline
  - Custom wallet address and contact info
  - Per-feature enable/disable flags

- **Network**
  - C2 status reporting
  - Remote unlock command checking
  - Periodic heartbeat with lock state

---

## Screenshots

![Lock Screen](docs/screenshots/lockscreen.png)
![Unlock Dialog](docs/screenshots/unlock-dialog.png)

---

## Project Structure

```
src/ScreenLocker/
├── Forms/              # Lock screen form, unlock dialog, countdown panel
├── Protection/         # Input blocker, task manager, Alt+Tab, process guard
├── Config/             # Lock config, message config (JSON-based)
├── Persistence/        # Registry shell override, startup registration
├── Models/             # Lock state, payment info
├── Utils/              # Screen helper, crypto wallet display, timer manager
└── Network/            # C2 status reporter
```

---

## Build Instructions

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10/11
- Windows Forms workload

### Build

```bash
dotnet restore
dotnet build -c Release
```

### Publish

```bash
dotnet publish -c Release -r win-x64 --self-contained -o publish/
```

---

## Usage

```bash
# Run with default configuration
ScreenLocker.exe

# Default unlock code: UNLOCK2024
```

### Configuration (lock_config.json)

```json
{
  "unlockCode": "UNLOCK2024",
  "lockDuration": "3.00:00:00",
  "blockTaskManager": true,
  "blockAltTab": true,
  "killDangerousProcesses": true,
  "preventSafeBoot": false,
  "autoStartOnBoot": true
}
```

### Message Configuration (message_config.json)

```json
{
  "title": "YOUR COMPUTER IS LOCKED",
  "message": "Pay 0.1 BTC to unlock...",
  "walletAddress": "bc1q...",
  "contactEmail": "unlock@protonmail.ch",
  "footerText": "[EDUCATIONAL PoC]"
}
```

---

## Disclaimer

This project is provided for **educational and authorized security research purposes only**. It is designed for:

- Understanding screen locker malware behavior and techniques
- Testing endpoint protection and EDR detection capabilities
- Security awareness training demonstrations
- Incident response procedure development

**DO NOT deploy this on systems you do not own or have explicit authorization to test.** Screen locking without consent is illegal in most jurisdictions. The default unlock code is `UNLOCK2024` for safe testing. The authors assume no liability for misuse.

**ALWAYS test in an isolated virtual machine with snapshots.**

---

## License

MIT License - See [LICENSE](LICENSE) for details.
