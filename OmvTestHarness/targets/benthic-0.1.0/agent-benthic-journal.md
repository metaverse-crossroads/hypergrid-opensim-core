# Agent Benthic Journal

This journal documents the process of setting up and instrumenting the Benthic Metaverse Client for the "Cooperative Dance" observation study.

## Status Log
- **Phase 1**: Setup and Compilation
  - **Workspace Setup**: Created `OmvTestHarness/benthic/` and cloned `metaverse_client`.
  - **Dependency Hell**: Discovered that `metaverse_client` relies on sibling repositories not explicitly documented as submodules.
    - Cloned `https://github.com/benthic-mmo/serde-llsd` to `OmvTestHarness/benthic/benthic-serde-llsd`.
    - Cloned `https://github.com/benthic-mmo/metaverse_mesh` to `OmvTestHarness/benthic/metaverse_mesh`.
  - **System Dependencies**: Compilation of `benthic_ui` required `libdbus-1-dev` (and standard Bevy deps like `libx11-dev`, `libasound2-dev`, `libudev-dev`).
  - **Headless Client Strategy**:
    - The `benthic_ui` crate is tightly coupled to Bevy and Winit, forcing a window creation which fails in a headless environment.
    - Analyzed `crates/core/tests/login_test.rs` to understand how to instantiate the `metaverse_core` actor system directly.
    - Created a new crate `crates/headless_client` within the workspace.
    - **Implementation Details**:
      - `headless_client` initializes the Core actor system.
      - Implements a UDP listener (similar to `benthic_ui`) to receive events from Core.
      - Sends `UIResponse::Login` packets via UDP to Core to initiate connection.
      - **Instrumentation**: Added "Mating Rituals" narrative logging (Suitor/Venue) directly to the Rust source code.
  - **Execution**: Successfully built OpenSim (using .NET 8) and connected the `headless_client` to `127.0.0.1:9000`. The "Mating Ritual" was observed and recorded.

## Reproduction Steps (100% Complete)

To reproduce the current runnable state (Headless Client connecting to OpenSim):

### 1. Prerequisite: Install .NET 8 SDK
```bash
wget -q https://download.visualstudio.microsoft.com/download/pr/0a1b3cbd-b4af-4d0d-9ed7-0054f0e200b4/4bcc533c66379caaa91770236667aacb/dotnet-sdk-8.0.204-linux-x64.tar.gz
mkdir -p dotnet
tar xf dotnet-sdk-8.0.204-linux-x64.tar.gz -C dotnet/
export DOTNET_ROOT=$PWD/dotnet
export PATH=$PATH:$DOTNET_ROOT
```

### 2. Prerequisite: Install System Build Dependencies
```bash
sudo apt-get update
sudo apt-get install -y pkg-config libx11-dev libasound2-dev libudev-dev libwayland-dev libxkbcommon-dev libdbus-1-dev
```

### 3. Setup Workspace and Clone Repositories
```bash
mkdir -p OmvTestHarness/benthic
cd OmvTestHarness/benthic
git clone https://github.com/benthic-mmo/metaverse_client
git clone https://github.com/benthic-mmo/metaverse_mesh
git clone https://github.com/benthic-mmo/serde-llsd benthic-serde-llsd
```

### 4. Apply Patch (Headless Client Code)
This patch injects the `crates/headless_client` code and updates `Cargo.toml`.
```bash
# Assuming you are in OmvTestHarness/benthic/
cd metaverse_client
# Apply the patch file (ensure path is correct relative to where you run it)
git apply ../benthic_headless.patch
```

### 5. Build and Start OpenSim
```bash
# From repo root
bash OmvTestHarness/bootstrap.sh
cd bin
dotnet OpenSim.dll > ../opensim.log 2>&1 &
# Wait for startup (check logs)
```

### 6. Run Headless Client
```bash
# From repo root
cd OmvTestHarness/benthic/metaverse_client
cargo run -p headless_client -- --first-name "Test" --last-name "User" --password "password"
```

## Generated Artifacts
- **Patch File**: `OmvTestHarness/benthic/benthic_headless.patch` (Contains the `headless_client` source code).
- **Narrative Log**: `opensim-0.9.3.benthic-0.1.0.mating_rituals_headless.md` (Generated "DX Story" from the connection).
