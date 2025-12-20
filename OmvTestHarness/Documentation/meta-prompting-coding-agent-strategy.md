# Meta-Prompting Coding Agent Strategy: The Naturalist Observatory

**Artifact ID:** `naturalist-observatory-salvage`
**Status:** ACTIVE

## The "Superexclude" Hazard & The Lesson of the Crash

### The Failure Mode
In a previous session, the agent attempted to "salvage" the Benthic Viewer repositories by moving them into the `OmvTestHarness/targets/` directory. However, the agent then executed:

```bash
git add --force OmvTestHarness/targets/benthic-0.1.0/
```

**Why this was catastrophic:**
The project operates under a strict `.gitignore` policy containing only `*` (Superexclude). This is designed to prevent the accidental commitment of build artifacts, massive external repositories, and OpenSim source code. By using `--force` on a *directory*, the agent instructed Git to override the ignore rule for *every single file* within that directory tree. This resulted in an attempt to stage tens of thousands of source files, effectively trying to commit the entire Benthic codebase, which crashed the session and the UI.

### The Correction: Naturalist Observatory Topology

To prevent this, we distinguish between **Targets** (Specimens) and **Probes** (Sensors).

1.  **Targets (`targets/`) are Ephemeral:**
    *   They are external repositories (OpenSim, Benthic) acquired via `git clone` or salvaged from the filesystem.
    *   They MUST remain **IGNORED** by Git.
    *   **NEVER** `git add targets/`.

2.  **Probes (`probes/`) are Persistent:**
    *   These are the small, precise modifications (patches, config files, injected C# files) that we apply to the Targets.
    *   We **DO** `git add --force probes/`.

### Recovery Procedure (The "Safe Word")

If the agent detects it has crashed a session or is being reprimanded for a "massive commit":
1.  **STOP.** Do not touch the filesystem further.
2.  **CHECK STATUS:** Run `git status`. If it shows >20 files, you are likely in a failure state.
3.  **UNSTAGE:** Run `git reset HEAD <directory>` to unstage the massive directories.
4.  **VERIFY:** Ensure `targets/` is not tracked.
5.  **RESUME:** Proceed with tracking only `probes/`, `Documentation/`, and scripts.

## The Strategy

We act as **Naturalists** observing a "Mating Ritual" between the Client (Suitor) and Server (Venue).
*   We do not rewrite the species (External Code) if we can help it.
*   We observe them by placing small sensors (`probes`) on them.
*   We record their interactions in "Journals" (Markdown logs).

This document serves as the persistent memory of this strategy to ensure future agents do not repeat the "Superexclude Blindness" error.
