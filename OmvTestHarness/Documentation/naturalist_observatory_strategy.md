# Naturalist Observatory Strategy

**Mission:** To establish a standalone, autonomous research observatory for discerning critical negotiations and communications patterns between Client and Server realms in the Viewer/OpenSim universe.

**Philosophy:** The "Naturalist Observer" approach utilizes benign logging probes to capture the raw, uninterpreted signals of protocol interactions. These signals are framed as "Mating Rituals"—a cooperative dance between the **Suitor** (Client) and the **Venue** (Server)—to provide accessible, narrative-driven insights into the emergent behavior of these complex systems.

## 1. Project Topology: The "Observatory"

The `OmvTestHarness/` directory serves as the virtual root of the Observatory. It is designed to be self-contained, managing its own targets and probes, ultimately independent of the surrogate repository it currently inhabits.

### Directory Structure

```text
OmvTestHarness/
├── Documentation/       # Manifests, Strategies, and Journals
├── targets/             # External projects (The "Specimens")
│   ├── benthic-0.1.0/   # Specific version of Benthic Viewer components
│   └── opensim-0.9.3/   # (Future) Specific version of OpenSim
├── probes/              # Instrumentation Logic (The "Sensors")
│   ├── benthic-0.1.0/   # Patches/Probes for Benthic
│   └── opensim-0.9.3/   # Patches/Probes for OpenSim
├── observatory_init.sh  # Acquisition script (Clones targets)
├── observatory_bootstrap.sh # Build script (Applies probes & compiles)
└── run_scenarios.sh     # Orchestrator (Conducts experiments)
```

## 2. The Liberation Plan

To transition from a "devfork" to an independent observatory, we adopt the following principles:

1.  **Acquisition over Embedding:** External projects (OpenSim, Viewers) are "Targets" to be acquired (via `git clone` or artifact download) into the `targets/` directory, not embedded as submodules.
2.  **Instrumentation via Injection:** Modifications are maintained as distinct "Probes" (patches or plugins) in the `probes/` directory. These are applied to Targets during the bootstrap phase.
3.  **Virtual Root:** All scripts and logic operate relative to `OmvTestHarness/`, treating it as the center of the universe.

## 3. Mating Rituals: The Protocol Narrative

The Observatory focuses on the "Cooperative Dance" of connection.

*   **The Suitor (Client):** The active agent seeking entry.
    *   *Behaviors:* Approaches, Offers Credentials, Extends Hand (UDP), Dances (Heartbeat), Leaves.
*   **The Venue (Server):** The passive host managing the space.
    *   *Behaviors:* Validates, Provisions (Circuit), Welcomes (Handshake), Streams (World Data), Timeouts.

### Instrumentation Points ("Camping Spots")

*   **Login Service (XML-RPC):** The Gatekeeper.
*   **UDP Server (Connect):** The Handshake.
*   **ClientView (World):** The Stream.

## 4. Execution Workflow

1.  **Init:** `observatory_init.sh` ensures all Targets are acquired.
2.  **Bootstrap:** `observatory_bootstrap.sh` applies Probes to Targets and compiles them.
3.  **Observation:** `run_scenarios.sh` launches the Venue and Suitor, recording the Mating Ritual to a centralized log.
4.  **Storytelling:** `generate_story.py` transforms the raw log into a human-readable "DX Story".
