#!/bin/bash
set -e

# Always assume running from repo root
if [ ! -f "runprebuild.sh" ]; then
    echo "Error: Must run from repository root"
    exit 1
fi

echo "--- Bootstrapping OpenSim & OmvTestHarness ---"

# 1. Prevent git pollution
echo '*' > .git/info/exclude

# 2. Prebuild
echo "--- Running Prebuild ---"
./runprebuild.sh

# 3. Build OpenSim
echo "--- Building OpenSim ---"
dotnet build --configuration Release OpenSim.sln

# 4. Build Test Harness
echo "--- Building OmvTestHarness ---"
dotnet build OmvTestHarness/OmvTestHarness.csproj

# 5. Configuration & DLLs
echo "--- Configuring ---"
# DLL Patch
cp bin/System.Drawing.Common.dll.linux bin/System.Drawing.Common.dll

# Config Copies
# Force copy if missing, but careful not to overwrite if user modified?
# Plan said: "Copy Examples"
[ ! -f bin/OpenSim.ini ] && cp bin/OpenSim.ini.example bin/OpenSim.ini
[ ! -f bin/config-include/StandaloneCommon.ini ] && cp bin/config-include/StandaloneCommon.ini.example bin/config-include/StandaloneCommon.ini
[ ! -f bin/Regions/Regions.ini ] && cp bin/Regions/Regions.ini.example bin/Regions/Regions.ini

# Automated Estate Setup (Idempotent sed)
# These seds only work if the lines are commented out (starts with ;)
# If run multiple times, they won't re-apply if already uncommented.
sed -i 's/; DefaultEstateName = My Estate/DefaultEstateName = My Estate/' bin/OpenSim.ini
sed -i 's/; DefaultEstateOwnerName = FirstName LastName/DefaultEstateOwnerName = Test User/' bin/OpenSim.ini
sed -i 's/; DefaultEstateOwnerUUID = .*/DefaultEstateOwnerUUID = 00000000-0000-0000-0000-000000000000/' bin/OpenSim.ini
sed -i 's/; DefaultEstateOwnerEMail = .*/DefaultEstateOwnerEMail = test@example.com/' bin/OpenSim.ini
sed -i 's/; DefaultEstateOwnerPassword = .*/DefaultEstateOwnerPassword = password/' bin/OpenSim.ini

echo "--- Bootstrap Complete ---"
