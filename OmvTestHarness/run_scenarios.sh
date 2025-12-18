#!/bin/bash
set -e

# Orchestrator for Mating Rituals Scientific Corroboration

# Check root
if [ ! -f "runprebuild.sh" ]; then
    echo "Error: Must run from repository root"
    exit 1
fi

DOTNET_CMD="dotnet" # Assumes dotnet is in PATH or exported

# Function to run a scenario
run_scenario() {
    SCENARIO_NAME=$1
    SCENARIO_ARGS=$2

    echo "======================================================================"
    echo "Starting Scenario: $SCENARIO_NAME"
    echo "Args: $SCENARIO_ARGS"
    echo "======================================================================"

    # 1. Cleanup previous run logs and DB for fresh state
    rm -f bin/mating_rituals.log
    rm -f bin/opensim.log
    rm -f bin/OpenSim.db
    rm -f bin/OpenSim.Log.db

    # 2. Start OpenSim
    echo "[SCENARIO] Starting OpenSim..."
    cd bin
    # We run in background, redirecting stdout/stderr
    $DOTNET_CMD OpenSim.dll > opensim.log 2>&1 &
    OPENSIM_PID=$!
    cd ..

    echo "[SCENARIO] OpenSim PID: $OPENSIM_PID. Waiting for startup..."
    # Wait for startup (simple sleep for now, could be smarter by grepping log)
    sleep 15

    # 3. Run Harness
    echo "[SCENARIO] Running OmvTestHarness..."
    cd OmvTestHarness
    # Capture output to console and file? Just console for now, the Logger writes to bin/mating_rituals.log
    $DOTNET_CMD run -- $SCENARIO_ARGS || true # Allow failure (e.g. rejection)
    cd ..

    echo "[SCENARIO] Harness finished."

    if [ "$SCENARIO_NAME" = "ghost" ]; then
        echo "[SCENARIO] Ghost mode: Waiting for Server Timeout (70s)..."
        sleep 70
    fi

    # 4. Cleanup OpenSim
    echo "[SCENARIO] Stopping OpenSim..."
    kill $OPENSIM_PID || true
    wait $OPENSIM_PID || true

    # 5. Archive Logs & Generate Story
    TIMESTAMP=$(date +%Y%m%d%H%M%S) # Only for temp files or non-committed logs?
    # User said: "NEVER put date stamps in ANYTHING you ever produce for me EVER that is meant to be COMMITED AS EXAMPLE"
    # But for local run logs, it might be fine.
    # Actually, I will name them by scenario only, overwriting previous runs of that scenario.

    LOG_TARGET="bin/mating_rituals_${SCENARIO_NAME}.log"
    cp bin/mating_rituals.log "$LOG_TARGET"
    echo "[SCENARIO] Log saved to $LOG_TARGET"

    echo "[SCENARIO] Generating Story..."
    # Update generate_story.py to read from the specific log?
    # Or just let it read the default bin/mating_rituals.log before we overwrote it?
    # Wait, I copied it. generate_story.py reads bin/mating_rituals.log by default.
    # I should pass the scenario name to generate_story.py so it names the output file correctly.

    python3 OmvTestHarness/generate_story.py --scenario "$SCENARIO_NAME"

    echo "[SCENARIO] Complete: $SCENARIO_NAME"
    echo ""
}

# Ensure generate_story.py is updated to accept args (I will do this next)

# Scenarios
# 1. Success
run_scenario "success" "--mode standard"

# 2. Rejection (Bad Password)
run_scenario "rejection" "--mode rejection"

# 3. Ghost (Login then Exit)
run_scenario "ghost" "--mode ghost"

# 4. Wallflower (Login, Connect, No Heartbeat)
run_scenario "wallflower" "--mode wallflower"

echo "All scenarios complete."
