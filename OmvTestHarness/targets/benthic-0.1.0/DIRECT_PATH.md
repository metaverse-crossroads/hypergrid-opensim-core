# Direct Path: Benthic Headless Client Setup & Mating Rituals

This document outlines the **direct, error-free path** to setting up the Benthic Metaverse Client in a headless environment, connecting it to OpenSim, and observing the "Mating Rituals" (connection sequence).

**Guideline**: Always use `git add --force` when adding files in this repository to bypass strict `.gitignore` rules.

## 1. Environment Prerequisites

### 1.1 Install .NET SDK 8.0 (Required for OpenSim)
OpenSim requires .NET 8. Install it manually into a local directory:

```bash
wget -q https://download.visualstudio.microsoft.com/download/pr/0a1b3cbd-b4af-4d0d-9ed7-0054f0e200b4/4bcc533c66379caaa91770236667aacb/dotnet-sdk-8.0.204-linux-x64.tar.gz
mkdir -p dotnet
tar xf dotnet-sdk-8.0.204-linux-x64.tar.gz -C dotnet/
export DOTNET_ROOT=$PWD/dotnet
export PATH=$PATH:$DOTNET_ROOT
```

### 1.2 Install System Dependencies (Required for Benthic & Bevy)
Benthic's Rust compilation requires specific Linux libraries, particularly `libdbus-1-dev` for keyring support and standard Bevy dependencies.

```bash
sudo apt-get update
sudo apt-get install -y pkg-config libx11-dev libasound2-dev libudev-dev libwayland-dev libxkbcommon-dev libdbus-1-dev
```

## 2. Workspace Setup

Create the directory structure and clone the required repositories. Note that `metaverse_client` depends on sibling folders.

```bash
mkdir -p OmvTestHarness/targets/benthic-0.1.0
cd OmvTestHarness/targets/benthic-0.1.0

# Clone Core Client
git clone https://github.com/benthic-mmo/metaverse_client

# Clone Sibling Dependencies (Required for compilation)
git clone https://github.com/benthic-mmo/metaverse_mesh
git clone https://github.com/benthic-mmo/serde-llsd benthic-serde-llsd
```

## 3. Implement Headless Client

The upstream `benthic_ui` is a GUI application (Bevy/Winit) that fails in headless environments. You must implement a custom CLI crate (`headless_client`) that interfaces directly with `metaverse_core`.

### 3.1 Apply the Patch
Use the provided `headless_client.patch` (located in `../../probes/benthic-0.1.0/`) to inject the `crates/headless_client` code and update `Cargo.toml`.

**Important**: If you are generating this code from scratch, ensure you create `crates/headless_client/src/main.rs`, `crates/headless_client/Cargo.toml`, and update the root `Cargo.toml` workspace members.

```bash
cd metaverse_client
# Apply the patch
git apply ../../../probes/benthic-0.1.0/headless_client.patch
```

## 4. Build OpenSim

Build the OpenSim server using the harness bootstrap script.

```bash
# From repo root
bash OmvTestHarness/bootstrap.sh
```

## 5. Execution & Observation

### 5.1 Start OpenSim
Run OpenSim in the background from its `bin` directory (critical for config loading).

```bash
# From repo root
cd bin
nohup dotnet OpenSim.dll > ../opensim.log 2>&1 &
cd ..
# Wait ~5-10 seconds for "LOGINS ENABLED"
sleep 10
```

### 5.2 Run Headless Client
Execute the client using `cargo run`. Redirect output to capture the narrative logs.

```bash
cd OmvTestHarness/targets/benthic-0.1.0/metaverse_client
cargo run -p headless_client -- \
  --first-name "Test" \
  --last-name "User" \
  --password "password" \
  > ../mating_rituals.log 2>&1 &
```

### 5.3 Verify Output
Inspect `OmvTestHarness/targets/benthic-0.1.0/mating_rituals.log`. You should see narrative entries like:
- "The Suitor (Client) prepares to enter the Venue..."
- "The Venue welcomes the Suitor! The mating ritual begins."

## 6. Persisting Changes

When saving your work, **ALWAYS** use `--force` to ensure ignored files (like patches or logs inside build directories) are tracked.

```bash
git add --force OmvTestHarness/probes/benthic-0.1.0/headless_client.patch
git add --force OmvTestHarness/targets/benthic-0.1.0/DIRECT_PATH.md
```
