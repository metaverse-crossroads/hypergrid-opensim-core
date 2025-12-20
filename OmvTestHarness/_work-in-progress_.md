# Naturalist Observatory: Work In Progress

**Meta-Context:** This document captures the negotiated "Game Plan" for refactoring the `OmvTestHarness` project into a standalone "Naturalist Observatory". It serves as a recovery checkpoint in case of session termination.

## Mission
To establish a standalone, autonomous research observatory for discerning critical negotiations and communications patterns between Client and Server realms in the Viewer/OpenSim universe.

## Core Directives
1.  **Virtual Root:** `OmvTestHarness/` is the center of the universe. All scripts and paths must be relative to it.
2.  **Acquisition over Embedding:** External dependencies (OpenSim, Viewers) are "Targets" cloned into `targets/`, not submodules.
3.  **Instrumentation via Injection:** Modifications are applied as "Probes" (patches/plugins) from `probes/` at build time.
4.  **Git Hygiene (CRITICAL):**
    *   `.gitignore` must contain ONLY `*` (Superexclude).
    *   `.git/info/exclude` must NOT exist.
    *   All files must be staged with `git add --force`.
    *   Do not trust auto-staging; verify with `git status`.

## Milestone Punchlist

- [x] **Milestone 0: Git Hygiene**
    - Confirmed `.gitignore` contains `*`.
    - Confirmed `.git/info/exclude` is removed.
    - Confirmed `git status` is clean.

- [x] **Milestone 1: Strategy Document**
    - Created `OmvTestHarness/Documentation/naturalist_observatory_strategy.md`.

- [x] **Milestone 1.5: Meta-Capture**
    - Create this `_work-in-progress_.md` document.

- [x] **Milestone 2: Physical Restructuring (Salvage & Liberation)**
    - [x] Create `targets/benthic-0.1.0`, `probes/benthic-0.1.0`, `probes/opensim-0.9.3`.
    - [x] Move Benthic repos (`metaverse_client`, `metaverse_mesh`, `benthic-serde-llsd`) to `targets/benthic-0.1.0/`.
    - [x] Move Benthic patch to `probes/benthic-0.1.0/headless_client.patch`.
    - [x] Move `DIRECT_PATH.md` to `targets/benthic-0.1.0/` and update paths.
    - [x] Copy OpenSim reference files (`LLLoginService.cs`, `LLUDPServer.cs`, `LLClientView.cs`) to `probes/opensim-0.9.3/`.
    - [x] Delete old `OmvTestHarness/benthic` directory.
    - [x] Submit.

- [ ] **Milestone 3: Acquisition & Foundation Verification**
    - [ ] Create `OmvTestHarness/observatory_init.sh` to verify/clone targets.
    - [ ] Verify script detects salvaged repos.
    - [ ] Submit.

- [ ] **Milestone 4: Execution & Observation**
    - [ ] Create `OmvTestHarness/observatory_bootstrap.sh` (Builds Benthic & Harness).
    - [ ] Refactor `OmvTestHarness/run_scenarios.sh` for new topology.
    - [ ] Update `OmvTestHarness/agent-omv-journal.md`.
    - [ ] Execute `bootstrap` and `run_scenarios`.
    - [ ] Verify Mating Rituals logs and Story generation.
    - [ ] Submit.

## Target Architecture

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
