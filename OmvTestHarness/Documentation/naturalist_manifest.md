# Gentle-Naturalist Architectural Injection Manifest

**Objective:** To position "benign logging probes" at strategic architectural junctions to capture the raw, uninterpreted signals of the client-server mating rituals. These probes act as "camera feeds" recording the exact conversation (packets, methods, payloads) without hallucinating intent or skipping steps.

## Part I: OpenSim (The Server's Perspective)

**Strategy:** Inject probes at the "mouths and ears" of the server: the Login Service (XML-RPC), the UDP Server (Packet Switch), and the Capabilities Handlers (HTTP).

### 1. The Gatekeeper: Login Service (XML-RPC)
*   **Camping Spot 1: Incoming Login Request**
    *   **File:** `OpenSim/Services/LLLoginService/LLLoginService.cs`
    *   **Method:** `Login(...)`
    *   **Signal:** Recv XML-RPC `login_to_simulator`.
    *   **Capture:** Client Version, Name, Start Location, Mac, ID0.
*   **Camping Spot 2: Login Response**
    *   **File:** `OpenSim/Services/LLLoginService/LLLoginService.cs`
    *   **Method:** `Login(...)` (end of method)
    *   **Signal:** Send XML-RPC Response.
    *   **Capture:** CircuitCode, SessionID, RegionHandle, SeedCapability URL.

### 2. The Switchboard: UDP Server
*   **Camping Spot 3: Packet Arrival (Raw)**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs`
    *   **Method:** `PacketReceived(...)`
    *   **Signal:** Recv UDP Packet.
    *   **Capture:** Packet Type, Sequence Number, AgentID (if authenticated).
*   **Camping Spot 4: Circuit Establishment**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs`
    *   **Method:** `HandleUseCircuitCode(...)`
    *   **Signal:** Recv `UseCircuitCode` Packet.
    *   **Capture:** CircuitCode, SessionID, ID.
*   **Camping Spot 5: Completion of Movement**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs` (or via ClientView event)
    *   **Method:** `HandleCompleteAgentMovement(...)`
    *   **Signal:** Recv `CompleteAgentMovement` Packet.
    *   **Capture:** AgentID.

### 3. The Diplomat: ClientView (Outbound Signaling)
*   **Camping Spot 6: Region Handshake**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
    *   **Method:** `SendRegionHandshake(...)`
    *   **Signal:** Send `RegionHandshake` Packet.
    *   **Capture:** RegionFlags, SimName, SimOwner.
*   **Camping Spot 7: World Materialization**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
    *   **Method:** `SendLayerData(...)`
    *   **Signal:** Send `LayerData` Packet.
    *   **Capture:** Layer Type (Land/Wind).

### 4. The Private Line: Capabilities (HTTP)
*   **Camping Spot 8: Seed Capability Request**
    *   **File:** `OpenSim/Region/ClientStack/Linden/Caps/BunchOfCaps/BunchOfCaps.cs`
    *   **Method:** `SeedCapRequest(...)`
    *   **Signal:** HTTP GET to Seed URL.
    *   **Capture:** Requested Capability map.
*   **Camping Spot 9: Event Queue Poll**
    *   **File:** `OpenSim/Region/ClientStack/Linden/Caps/EventQueue/EventQueueGetHandlers.cs`
    *   **Method:** `Handler(...)`
    *   **Signal:** HTTP POST/GET to EventQueue URL.
    *   **Capture:** Ack/Done ID.
*   **Camping Spot 10: Event Queue Dispatch**
    *   **File:** `OpenSim/Region/ClientStack/Linden/Caps/EventQueue/EventQueueGetModule.cs`
    *   **Method:** `Enqueue(...)`
    *   **Signal:** Dispatch Event to Queue.
    *   **Capture:** Event Name (e.g., `EnableSimulator`, `EstablishAgentCommunication`).

### 5. Advanced Observations
*   **Camping Spot 11: Agent Update (Heartbeat)**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
    *   **Method:** `HandleAgentUpdate(...)`
    *   **Signal:** Recv `AgentUpdate` Packet.
    *   **Capture:** Camera Position, Agent Position, Flags.
*   **Camping Spot 12: Object Update (Visuals)**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
    *   **Method:** `SendEntityUpdate(...)` (or similar low-level packet sender)
    *   **Signal:** Send `ObjectUpdate` / `ImprovedTerseObjectUpdate`.
    *   **Capture:** LocalID, PCode.

---

## Part II: LibreMetaverse (The Client's Perspective)

**Strategy:** Inject probes at the "mouths and ears" of the client library: NetworkManager (Packet handling), Login (XML-RPC), and CapsClient (HTTP).

### 1. The Seeker: Login
*   **Camping Spot 13: Login Request**
    *   **File:** `LibreMetaverse/Login.cs`
    *   **Method:** `Login(...)`
    *   **Signal:** Send XML-RPC `login_to_simulator`.
    *   **Capture:** Grid URL, Credentials (redacted).
*   **Camping Spot 14: Login Response Processing**
    *   **File:** `LibreMetaverse/Login.cs`
    *   **Method:** `ResponseHandler`
    *   **Signal:** Recv XML-RPC Response.
    *   **Capture:** Login Success/Fail, AgentID, SessionID, CircuitCode.

### 2. The Connector: NetworkManager
*   **Camping Spot 15: UDP Connection Start**
    *   **File:** `LibreMetaverse/NetworkManager.cs`
    *   **Method:** `Connect(...)`
    *   **Signal:** Open UDP Socket.
    *   **Capture:** Sim IP:Port.
*   **Camping Spot 16: Sending Circuit Code**
    *   **File:** `LibreMetaverse/NetworkManager.cs` (or wherever UseCircuitCode is constructed)
    *   **Method:** `Login(...)` (sequence)
    *   **Signal:** Send `UseCircuitCode`.
    *   **Capture:** CircuitCode, ID, SessionID.
*   **Camping Spot 17: Packet Reception**
    *   **File:** `LibreMetaverse/NetworkManager.cs`
    *   **Method:** `PacketReceived(...)` (or callback)
    *   **Signal:** Recv UDP Packet.
    *   **Capture:** Packet Type.

### 3. The Negotiator: Simulator & Caps
*   **Camping Spot 18: Region Handshake Handling**
    *   **File:** `LibreMetaverse/Simulator.cs` (or PacketHandler)
    *   **Method:** `RegionHandshakeHandler(...)`
    *   **Signal:** Recv `RegionHandshake`.
    *   **Capture:** RegionID, SimName.
*   **Camping Spot 19: Seed Capability Request**
    *   **File:** `LibreMetaverse/Simulator.cs` (or CapsClient)
    *   **Method:** `RequestCapabilities(...)`
    *   **Signal:** HTTP GET SeedCap.
    *   **Capture:** Requested Caps List.
*   **Camping Spot 20: Event Queue Listener**
    *   **File:** `LibreMetaverse/Capabilities/EventQueueClient.cs`
    *   **Method:** `EventQueueHandler(...)`
    *   **Signal:** Recv Event via HTTP.
    *   **Capture:** Event Name (e.g., `TeleportFinish`, `CrossedRegion`).

### 4. The Observer: World State
*   **Camping Spot 21: Movement Complete**
    *   **File:** `LibreMetaverse/AgentManager.cs`
    *   **Method:** `CompleteAgentMovement(...)`
    *   **Signal:** Send `CompleteAgentMovement`.
    *   **Capture:** Triggered after RegionHandshake/SeedCap.
*   **Camping Spot 22: Terrain Patch Reception**
    *   **File:** `LibreMetaverse/TerrainManager.cs`
    *   **Method:** `LayerDataHandler(...)`
    *   **Signal:** Recv `LayerData`.
    *   **Capture:** Patch Coordinates.
*   **Camping Spot 23: Object Update Reception**
    *   **File:** `LibreMetaverse/ObjectManager.cs`
    *   **Method:** `ObjectUpdateHandler(...)`
    *   **Signal:** Recv `ObjectUpdate`.
    *   **Capture:** LocalID, Name.
*   **Camping Spot 24: Logout**
    *   **File:** `LibreMetaverse/NetworkManager.cs`
    *   **Method:** `Logout(...)`
    *   **Signal:** Send `LogoutRequest`.
    *   **Capture:** Session End.

---

## Log Format
All probes will utilize a standardized format to ensure the "stereo" feeds can be synchronized.

`[MATING RITUAL] [TIMESTAMP] [SIDE] [COMPONENT] [SIGNAL] [PAYLOAD]`

*   **SIDE:** `SERVER` or `CLIENT`
*   **COMPONENT:** `LOGIN`, `UDP`, `CAPS`, `WORLD`
*   **SIGNAL:** `RECV PacketType`, `SEND MethodName`, etc.
*   **PAYLOAD:** `Circuit=123`, `Region=Default`, etc.

**Example Stream:**
```
[SERVER] [LOGIN] RECV XML-RPC login_to_simulator | User: Test User
[SERVER] [LOGIN] SEND XML-RPC Response | Success, Circuit: 888
[CLIENT] [LOGIN] RECV XML-RPC Response | Success, Circuit: 888
[CLIENT] [UDP]   SEND Packet UseCircuitCode | Circuit: 888
[SERVER] [UDP]   RECV Packet UseCircuitCode | Circuit: 888
[SERVER] [UDP]   SEND Packet RegionHandshake | Region: Default
[CLIENT] [UDP]   RECV Packet RegionHandshake | Region: Default
[CLIENT] [CAPS]  SEND HTTP GET SeedCap | URL: http://...
[SERVER] [CAPS]  RECV HTTP GET SeedCap | Agent: ...
```
