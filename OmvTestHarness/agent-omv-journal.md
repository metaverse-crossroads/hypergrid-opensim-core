# OpenSim & OmvTestHarness Bootstrapping Journal

This guide describes how to build, configure, and verify OpenSim and the client test harness using the "Scientific Corroboration" suite.

## 1. Quick Start (Bootstrap)

A single script is provided to handle the entire build and configuration process (Prebuild, Build, DLL patching, Config setup).

```bash
# Must be run from the repository root
./OmvTestHarness/bootstrap.sh
```

**What it does:**
1.  Cleans `.git` pollution (via `info/exclude`).
2.  Runs `./runprebuild.sh`.
3.  Builds `OpenSim.sln`.
4.  Builds `OmvTestHarness/OmvTestHarness.csproj`.
5.  Copies necessary `bin/` config files (`OpenSim.ini`, etc.) and patches `System.Drawing.Common.dll`.
6.  Configures `OpenSim.ini` for automated Estate setup (Test User / password).

## 2. Running Scenarios (Scientific Corroboration)

To verify the "Mating Rituals" (Client/Server handshake) under various conditions, use the scenario orchestrator:

```bash
./OmvTestHarness/run_scenarios.sh
```

**Orchestrated Scenarios:**
1.  **Success ("The Consummation")**: Standard login, handshake, world entry, and graceful logout.
2.  **Rejection ("The Closed Door")**: Login attempt with invalid password.
3.  **Ghost ("The Vanishing")**: Login success (HTTP), but the client vanishes before/during the UDP connection.
    *   *Observation*: The server reserves the circuit but eventually times out the ghost session. Re-login is blocked until cleanup.
4.  **Wallflower ("The Passive Observer")**: Client logs in and connects, but explicitly disables `AgentUpdate` (Heartbeat) and `Ping` packets.
    *   *Observation*: The connection **remains active** as long as the server streams data (e.g., Terrain/Objects) and the client ACKs it. Silence != Disconnection.

## 3. The "DX Story" Output

The orchestrator generates Markdown narratives for each scenario in the repository root (e.g., `opensim-0.9.3.libremetaverse-2.0.0.mating_rituals_ghost.md`).

These stories use a "Naturalist" tone to describe the protocol exchange as a cooperative dance between Suitor (Client) and Venue (Server).

### Example Excerpt (Ghost)
> **[SERVER] [LOGIN] CIRCUIT PROVISION**: The Venue reserves a spot on the dance floor (Region) and issues a unique ticket (CircuitCode).
> **[CLIENT] [BEHAVIOR] GHOST**: The Suitor vanishes immediately after the introduction, leaving the Venue waiting.
> **[SERVER] [UDP] TIMEOUT**: The Venue notices the Suitor has stopped moving (Heartbeat Timeout). The connection fades into silence.

## 4. Nuances & Fixes
-   **libgdiplus**: On systems without `libgdiplus`, OpenSim map generation crashes. Code patches (try-catch blocks) in `VectorRenderModule.cs`, `MapImageService.cs`, and `MapImageModule.cs` prevent the server from crashing.
-   **Input Redirection**: Running OpenSim in background causes a CPU loop in `LocalConsole.cs` if `Console.ReadLine()` returns null. A patch in `LocalConsole.cs` handles `IsInputRedirected`.
-   **Platform Specifics**: Always copy `System.Drawing.Common.dll.linux` on Linux. Do not use the `OpenMetaverse.dll` from `bin/` for client development on Linux .NET 8; use `LibreMetaverse` (NuGet).
-   **Ghost Sessions**: If a client disconnects abruptly (Ghost scenario), OpenSim keeps the session active in the DB. This prevents immediate re-login ("You appear to be already logged in"). The scenario runner cleans `bin/OpenSim.db` to ensure a fresh state for each run.
