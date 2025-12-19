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
      - **Fixes**: Corrected imports (e.g., `Packet` moved to `packet::Packet`, `PacketType` replaced by `UIMessage`), and removed invalid field access (`success`, `message`) on `LoginResponse`.
  - **Current Status**: `headless_client` compiles successfully. OpenSim build pending .NET installation.

## Key Findings
- The Benthic codebase is in active development. `metaverse_core` was likely renamed from `metaverse_session` in the past, but tests still reference the old name.
- The `initialize` function signature in `core` changed to accept ports (`u16`) instead of file paths, implying a switch from UDS to UDP/TCP for local IPC.
- `LoginResponse` struct does not contain a success boolean; existence implies success (or fields were removed).

## Next Steps
1. Install .NET SDK 8.0.
2. Build OpenSim via `bootstrap.sh`.
3. Verify `headless_client` connection to local OpenSim.
4. Add "Mating Rituals" instrumentation.
