# OpenSim Client/Server Mating Rituals: The Wallflower

*This document captures the raw "DX story" of the connection sequence between an OpenSim Server and a LibreMetaverse Client, observed by a neutral naturalist. It utilizes "benign logging probes" to record the signals exchanged during this digital dance.*

## Prologue: The Environment
- **Server**: OpenSim (Instrumented)
- **Client**: LibreMetaverse Test Harness (Instrumented)
- **Protocol**: HTTP (XML-RPC), UDP, HTTP (Caps)
- **Scenario**: `wallflower`

---

## Part I: The Overture (Login)
1.  **[CLIENT] [LOGIN] START**: The Suitor (Client) approaches the Venue, presenting credentials for `Test User`. (*URI: http://localhost:9000/, User: Test User, Mode: wallflower*)
1.  **[SERVER] [LOGIN] RECV XML-RPC login_to_simulator**: The Gatekeeper (Login Service) receives the formal request. It examines the suitor's identity and viewer signature. (*User: Test User, Viewer: 1.0.0, Channel: OmvTestHarness, IP: 127.0.0.1:49766*)
1.  **[SERVER] [LOGIN] AUTH SUCCESS**: The Gatekeeper nods in approval. The credentials match the guest list. (*User: Test User*)
1.  **[SERVER] [LOGIN] CIRCUIT PROVISION**: The Venue reserves a spot on the dance floor (Region) and issues a unique ticket (CircuitCode). (*Circuit: 1966132762, Region: Default Region*)
1.  **[SERVER] [LOGIN] SEND XML-RPC Response**: The Gatekeeper hands the Suitor the invitation (Response), containing the CircuitCode and the location of the dance floor. (*Success*)
1.  **[CLIENT] [LOGIN] PROGRESS ConnectingToSim**: The Suitor accepts the invitation and turns towards the dance floor. (*Connecting to simulator...*)

## Part II: The Approach (Handshake)
1.  **[SERVER] [UDP] RECV UseCircuitCode**: The Venue accepts the hand. It checks the ticket (CircuitCode) against its reservation list. (Details: `CircuitCode: 1966132762, Session: cb5ce6d7-c4ea-491c-b0db-6dcce167594d`)
1.  **[SERVER] [UDP] SEND RegionHandshake**: The Venue pulls the Suitor close, whispering the rules of the house (RegionHandshake). (*Region: Default Region*)

## Part IV: The Dance (World Stream)
1.  **[SERVER] [UDP] SEND AgentMovementComplete**: The Venue guides the Suitor to their starting position. (Position: `Pos: <128, 128, 26.034172>, Look: <0.99, 0.042, 0>`)
1.  **[CLIENT] [UDP] CONNECTED**: The Suitor extends a hand (UDP Socket) towards the Venue's IP. (*Sim: Default Region, IP: 192.168.0.2:9000*)
1.  **[CLIENT] [BEHAVIOR] WALLFLOWER**: The Suitor enters the floor but stands perfectly still, refusing to engage in the rhythm (Heartbeat suppressed). (*Disabling Agent Updates (Heartbeat)*)
1.  **[CLIENT] [BEHAVIOR] WALLFLOWER**: The Suitor enters the floor but stands perfectly still, refusing to engage in the rhythm (Heartbeat suppressed). (*Waiting for server timeout...*)
1.  **[CLIENT] [UDP] RECV ObjectUpdate**: The Venue reveals the other dancers and decorations (ObjectUpdates). (*Size: 322*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 923*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 944*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 912*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 940*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 904*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 963*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 904*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 904*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 906*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 908*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 273*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 551*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 720*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 762*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 701*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 620*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 644*)
1.  **[CLIENT] [UDP] RECV LayerData**: The Venue unrolls the carpet (Terrain Data) beneath the Suitor's feet. (*Size: 658*)
---

*Generated by MatingRitualLogger instrumentation.*