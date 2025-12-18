# OpenSim & OmvTestHarness Bootstrapping Journal

This guide describes how to build, configure, and verify OpenSim and the client test harness from a fresh checkout of this branch.

## 1. Initial Setup

**Crucial**: Prevent build artifacts from polluting the git status.
```bash
echo '*' > .git/info/exclude
```

## 2. Prerequisites
- Linux Environment
- .NET 8 SDK (Installed and verified)

## 3. Build OpenSim
1.  **Generate Project Files**:
    ```bash
    ./runprebuild.sh
    ```
    *Note: If `dotnet` is not in PATH, use `DOTNET_ROOT=/path/to/dotnet ./runprebuild.sh`.*

2.  **Build Solution**:
    ```bash
    dotnet build --configuration Release OpenSim.sln
    ```

3.  **Linux Compatibility Fix**:
    Replace the Windows-specific System.Drawing library with the Linux version provided.
    ```bash
    cp bin/System.Drawing.Common.dll.linux bin/System.Drawing.Common.dll
    ```

## 4. OpenSim Configuration
Copy the example configuration files and configure for automated startup (non-interactive).

1.  **Copy Examples**:
    ```bash
    cp bin/OpenSim.ini.example bin/OpenSim.ini
    cp bin/config-include/StandaloneCommon.ini.example bin/config-include/StandaloneCommon.ini
    cp bin/Regions/Regions.ini.example bin/Regions/Regions.ini
    ```

2.  **Configure Automated Estate Setup**:
    Edit `bin/OpenSim.ini` to uncomment and set the `[Estates]` section. This prevents the interactive prompt on first run.

    *Command to apply changes:*
    ```bash
    sed -i 's/; DefaultEstateName = My Estate/DefaultEstateName = My Estate/' bin/OpenSim.ini
    sed -i 's/; DefaultEstateOwnerName = FirstName LastName/DefaultEstateOwnerName = Test User/' bin/OpenSim.ini
    sed -i 's/; DefaultEstateOwnerUUID = .*/DefaultEstateOwnerUUID = 00000000-0000-0000-0000-000000000000/' bin/OpenSim.ini
    sed -i 's/; DefaultEstateOwnerEMail = .*/DefaultEstateOwnerEMail = test@example.com/' bin/OpenSim.ini
    sed -i 's/; DefaultEstateOwnerPassword = .*/DefaultEstateOwnerPassword = password/' bin/OpenSim.ini
    ```

## 5. Client Test Harness (OmvTestHarness)
The `OmvTestHarness` is a C# console application used to verify connectivity.

### Nuances & Fixes
-   **Library Switch**: The harness uses `LibreMetaverse` (via NuGet) instead of the local `OpenMetaverse.dll` found in `bin/`. The local DLL causes a `PlatformNotSupportedException` (Socket.IOControl) on Linux .NET 8. `LibreMetaverse` 2.0+ is compatible.
-   **Configuration**: Requires `System.Configuration.ConfigurationManager` and `log4net`.

### Build Harness
```bash
dotnet build OmvTestHarness/OmvTestHarness.csproj
```

## 6. Verification Run

1.  **Start OpenSim Server**:
    Run OpenSim in the background. Redirect output to monitor log.
    ```bash
    cd bin
    dotnet OpenSim.dll > opensim.log 2>&1 &
    cd ..
    ```
    *Wait ~10-15 seconds for startup. Check `bin/opensim.log` for "INITIALIZATION COMPLETE".*

2.  **Run Client Harness**:
    ```bash
    cd OmvTestHarness
    dotnet run
    ```

3.  **Expected Output**:
    The client should connect, log in, display Agent/Session IDs, and then log out.
    ```
    Attempting to login to http://localhost:9000/ as Test User...
    Login Progress: ConnectingToSim - Connecting to simulator...
    ...
    Login Successful!
    Agent ID: ...
    Session ID: ...
    Sim Name: Default Region
    ...
    Logged out.
    ```

## 7. Mating Rituals Investigation
We have instrumented OpenSim with a `MatingRitualLogger` to capture the narrative of the client/server connection process.

### Instrumentation Points
-   **`OpenSim/Services/LLLoginService/LLLoginService.cs`**: Captures the initial login request and authentication (Part I).
-   **`OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs`**: Captures the `UseCircuitCode` packet and UDP connection establishment (Part II).
-   **`OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`**: Captures the `RegionHandshake`, `AgentMovement`, and initial world data transmission (Parts III & IV).

### Reproduction
1.  Ensure OpenSim is built with the instrumentation changes.
2.  Start OpenSim.
3.  Run the `OmvTestHarness`.
4.  Inspect `bin/opensim.log` (or console output) for `[MATING RITUAL]` tags.
5.  A summary of the observed rituals is available in `OmvTestHarness/opensim-0.9.3.libremetaverse-2.0.0.mating_rituals.md`.

## 8. Lessons Learned / Troubleshooting
-   **libgdiplus**: On systems without `libgdiplus`, OpenSim map generation crashes. Code patches (try-catch blocks) in `VectorRenderModule.cs`, `MapImageService.cs`, and `MapImageModule.cs` prevent the server from crashing, though map tiles won't generate.
-   **Input Redirection**: Running OpenSim in background causes a CPU loop in `LocalConsole.cs` if `Console.ReadLine()` returns null. A patch in `LocalConsole.cs` handles `IsInputRedirected`.
-   **Platform Specifics**: Always copy `System.Drawing.Common.dll.linux` on Linux. Do not use the `OpenMetaverse.dll` from `bin/` for client development on Linux .NET 8; use `LibreMetaverse`.
-   **Git Exclude**: Use `echo '*' > .git/info/exclude` to keep the working directory clean of build artifacts without modifying `.gitignore`.
