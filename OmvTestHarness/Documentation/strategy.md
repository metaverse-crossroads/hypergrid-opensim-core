# Mating Rituals Strategy

## Objective
To capture a "human-friendly", neutral observer story of the connection process between the OpenSim server and the LibreMetaverse client.

## Analysis of Key Connection Points

### 1. The Initial Handshake (XML-RPC/HTTP)
**Location:** `OpenSim/Services/LLLoginService/LLLoginService.cs`
- **Method:** `Login(...)`
- **Significance:** This is where the client first knocks on the door. It provides credentials, and the server validates them, checks the grid status, and prepares a "circuit" for the agent.
- **Story Element:** "The Client approaches the Grid Gatekeeper, presenting credentials for 'Test User'. The Gatekeeper verifies the identity against the User Account Service..."

### 2. The UDP Connection (UseCircuitCode)
**Location:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs`
- **Method:** `HandleUseCircuitCode(...)`
- **Significance:** The client uses the ticket provided by the Login Service to establish a UDP connection with a specific Region Server.
- **Story Element:** "Having received a circuit code, the Client initiates a UDP connection to the Region. The Region Server recognizes the code and welcomes the Client, creating a presence."

### 3. The Region Handshake & Entry
**Location:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
- **Method:** `SendRegionHandshake()` and `MoveAgentIntoRegion(...)`
- **Significance:** The server describes the rules of the world (RegionInfo) and physically places the avatar into the scene.
- **Story Element:** "The Region Server extends a hand, detailing the laws of the land (Region Flags). The Avatar is then materialized at position (128, 128)."

### 4. World Loading (Packet Exchange)
**Location:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
- **Methods:** `SendLayerData()`, `SendMapBlock()`, `SendInventoryFolderDetails()`
- **Significance:** The server streams data to the client to render the world (terrain, objects, inventory).
- **Story Element:** "The Server begins to paint the landscape, sending terrain patches to the Client. Simultaneously, the Client's inventory skeleton is transmitted."

## Proposed Logging Strategy

I will create a helper class `OpenSim/Region/ClientStack/Linden/UDP/MatingRitualLogger.cs` (or similar) to centralize this storytelling logging. This logger will format messages in a specific "Story" format, distinct from standard debug logs.

I will inject calls to this logger at the key points identified above.

**Format:**
`[MATING RITUAL] <Actor> <Action> <Context>`

**Example:**
`[MATING RITUAL] Client 'Test User' -> Initiates Login Request (Viewer: OpenMetaverse)`
`[MATING RITUAL] Server -> Validates Credentials (Success)`
`[MATING RITUAL] Server -> Provisions Circuit (Code: 12345)`
`[MATING RITUAL] Client -> Connects UDP (Circuit: 12345)`
`[MATING RITUAL] Server -> Acknowledges Connection (AgentID: ...)`

## Implementation Plan

1.  **Create `OpenSim/Framework/MatingRitualLogger.cs`**: A simple static class to handle the formatting and output of these specific logs. It might write to a separate file or just use a special prefix in the main log.
2.  **Instrument `LLLoginService.cs`**: Capture the XML-RPC login attempt and result.
3.  **Instrument `LLUDPServer.cs`**: Capture the `UseCircuitCode` packet handling.
4.  **Instrument `LLClientView.cs`**: Capture `SendRegionHandshake`, `CompleteAgentMovement`, and initial world data transmission.
5.  **Refine**: Run the harness and adjust the logging to ensure the "story" flows logically.

## Verification
Run `OmvTestHarness` and grep the logs for `[MATING RITUAL]`. The output should read like a narrative of the connection process.
