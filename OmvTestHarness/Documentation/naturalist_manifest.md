# Gentle-Naturalist Architectural Injection Manifest

**Objective:** To position "benign logging probes" at strategic architectural junctions to capture the raw, uninterpreted signals of the client-server mating rituals. These probes act as "camera feeds" recording the exact conversation (packets, methods, payloads) without hallucinating intent or skipping steps.

**Philosophy:** The connection process is viewed not as a mechanical handshake, but as a **Cooperative Dance** between two autonomous agents: the **Suitor** (Client) and the **Venue** (Server).

## Part I: OpenSim (The Venue's Perspective)

**Strategy:** Inject probes at the "mouths and ears" of the server: the Login Service (XML-RPC), the UDP Server (Packet Switch), and the Capabilities Handlers (HTTP).

### 1. The Gatekeeper: Login Service (XML-RPC)
*   **Camping Spot 1: Incoming Login Request**
    *   **File:** `OpenSim/Services/LLLoginService/LLLoginService.cs`
    *   **Signal:** `RECV XML-RPC login_to_simulator`
    *   **Narrative:** "The Gatekeeper receives the formal request."
*   **Camping Spot 2: Login Response**
    *   **File:** `OpenSim/Services/LLLoginService/LLLoginService.cs`
    *   **Signal:** `SEND XML-RPC Response`
    *   **Narrative:** "The Gatekeeper hands the Suitor the invitation."

### 2. The Switchboard: UDP Server
*   **Camping Spot 3: Circuit Establishment**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs`
    *   **Signal:** `RECV UseCircuitCode`
    *   **Narrative:** "The Venue accepts the hand."
*   **Camping Spot 4: Timeout/Disconnection**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLUDPServer.cs` (`DeactivateClientDueToTimeout`)
    *   **Signal:** `TIMEOUT`
    *   **Narrative:** "The connection fades into silence."

### 3. The Diplomat: ClientView (Outbound Signaling)
*   **Camping Spot 5: Region Handshake**
    *   **File:** `OpenSim/Region/ClientStack/Linden/UDP/LLClientView.cs`
    *   **Signal:** `SEND RegionHandshake`
    *   **Narrative:** "The Venue whispers the rules of the house."

---

## Part II: LibreMetaverse (The Suitor's Perspective)

**Strategy:** Inject probes at the "mouths and ears" of the client library via the `OmvTestHarness`.

### 1. The Seeker: Login
*   **Camping Spot 6: Login Start**
    *   **File:** `OmvTestHarness/Program.cs`
    *   **Signal:** `START`
    *   **Narrative:** "The Suitor approaches the Venue."

### 2. The Connector: NetworkManager
*   **Camping Spot 7: UDP Connection**
    *   **File:** `OmvTestHarness/Program.cs`
    *   **Signal:** `CONNECTED`
    *   **Narrative:** "The Suitor extends a hand."

### 3. Behavioral Modes
The harness supports specific behavioral deviations to test the Venue's patience:
*   **Ghost**: Vanish immediately after login.
*   **Wallflower**: Connect but suppress `AgentUpdate` (Heartbeat) and `Ping` packets.

---

## Log Format
All probes utilize a standardized format:
`[MATING RITUAL] [SIDE] [COMPONENT] [SIGNAL] [PAYLOAD]`

**Example Stream:**
```
[SERVER] [LOGIN] RECV XML-RPC login_to_simulator | User: Test User
[SERVER] [LOGIN] SEND XML-RPC Response | Success
[CLIENT] [UDP]   CONNECTED | Sim: Default Region
[SERVER] [UDP]   TIMEOUT | Agent: Test User
```
