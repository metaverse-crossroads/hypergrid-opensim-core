using System;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.Threading;
using log4net.Config;
using log4net;
using System.Reflection;
using System.IO;

namespace OmvTestHarness
{
    public static class MatingRitualLogger
    {
        private static string LogPath = "../bin/mating_rituals.log";

        public static void Log(string side, string component, string signal, string payload = "")
        {
            string message = $"[MATING RITUAL] [{side}] [{component}] {signal}";
            if (!string.IsNullOrEmpty(payload))
            {
                message += $" | {payload}";
            }

            Console.WriteLine(message);

            try
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
            }
            catch (Exception) { }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));

            string firstName = "Test";
            string lastName = "User";
            string password = "password";
            string loginURI = "http://localhost:9000/";
            string mode = "standard";

            // Parse Args
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--mode" && i + 1 < args.Length) mode = args[i + 1];
                if (args[i] == "--user" && i + 1 < args.Length) firstName = args[i + 1]; // Simple override, assumes "Last" name is fixed or handled otherwise, keeping it simple
                if (args[i] == "--password" && i + 1 < args.Length) password = args[i + 1];
            }

            if (mode == "rejection") password = "badpassword";

            MatingRitualLogger.Log("CLIENT", "LOGIN", "START", $"URI: {loginURI}, User: {firstName} {lastName}, Mode: {mode}");

            GridClient client = new GridClient();

            // Camping Spot 14: Login Response
            client.Network.LoginProgress += (sender, e) =>
            {
                MatingRitualLogger.Log("CLIENT", "LOGIN", $"PROGRESS {e.Status}", e.Message);
            };

            // Camping Spot 15: UDP Connection (SimConnected)
            client.Network.SimConnected += (sender, e) =>
            {
                MatingRitualLogger.Log("CLIENT", "UDP", "CONNECTED", $"Sim: {e.Simulator.Name}, IP: {e.Simulator.IPEndPoint}");

                if (mode == "wallflower")
                {
                    // In "Wallflower" mode, we want to connect but then silence the heartbeats
                    // LibreMetaverse usually sends AgentUpdate automatically. We need to suppress it.
                    // The easiest way is to set the update interval to infinity or very high.
                    // However, LibOMV settings are powerful.

                    // Actually, let's just NOT respond to anything.
                    // But LibOMV handles a lot in background threads.

                    // Let's log that we are going silent.
                    MatingRitualLogger.Log("CLIENT", "BEHAVIOR", "WALLFLOWER", "Disabling Agent Updates (Heartbeat)");
                    client.Settings.SEND_AGENT_UPDATES = false; // Don't send updates
                    client.Settings.SEND_PINGS = false; // Don't send pings
                }
            };

            // Camping Spot 18: Region Handshake
            client.Network.RegisterCallback(PacketType.RegionHandshake, (sender, e) =>
            {
                MatingRitualLogger.Log("CLIENT", "UDP", "RECV RegionHandshake", $"Size: {e.Packet.Length}");
            });

            // Camping Spot 22: LayerData (Terrain)
            client.Network.RegisterCallback(PacketType.LayerData, (sender, e) =>
            {
                MatingRitualLogger.Log("CLIENT", "UDP", "RECV LayerData", $"Size: {e.Packet.Length}");
            });

            // Camping Spot 23: ObjectUpdate
            client.Network.RegisterCallback(PacketType.ObjectUpdate, (sender, e) =>
            {
                MatingRitualLogger.Log("CLIENT", "UDP", "RECV ObjectUpdate", $"Size: {e.Packet.Length}");
            });

            // Event Queue (Camping Spot 20)
            client.Network.EventQueueRunning += (sender, e) =>
            {
                 MatingRitualLogger.Log("CLIENT", "CAPS", "EQ RUNNING", $"Sim: {e.Simulator.Name}");
            };

            LoginParams loginParams = client.Network.DefaultLoginParams(firstName, lastName, password, "OmvTestHarness", "1.0.0");
            loginParams.URI = loginURI;

            // Mode: Ghost - Disconnect immediately after HTTP login, before UDP?
            // LibOMV Login() does XMLRPC then connects UDP. It's a blocking call that does both.
            // To ghost, we might need to interrupt it, or...
            // Actually, we can just close the client right after login returns success?
            // The "Ghost" scenario implies we *don't* send UDP UseCircuitCode.
            // But LibOMV sends it inside Login().

            // For the sake of this harness, "Ghost" might just mean "Login, then Exit Immediately".
            // If we want to *truly* ghost (get Circuit but don't use it), we'd need to modify LibOMV or do manual XMLRPC.
            // Manual XMLRPC is too much work.
            // Let's stick to "Login Success -> Immediate Exit" which means the server sees a login but maybe the UDP connection is cut short.

            if (client.Network.Login(loginParams))
            {
                MatingRitualLogger.Log("CLIENT", "LOGIN", "SUCCESS", $"Agent: {client.Self.AgentID}");

                if (mode == "ghost")
                {
                    MatingRitualLogger.Log("CLIENT", "BEHAVIOR", "GHOST", "Vanishing immediately...");
                    Environment.Exit(0); // Harsh exit
                }

                if (mode == "wallflower")
                {
                    // Wait for the server to timeout us
                    MatingRitualLogger.Log("CLIENT", "BEHAVIOR", "WALLFLOWER", "Waiting for server timeout...");
                    Thread.Sleep(90000); // 90 seconds (Server timeout default is often 60s)
                }
                else
                {
                    // Standard stay connected for a bit
                    Thread.Sleep(5000);
                    MatingRitualLogger.Log("CLIENT", "LOGOUT", "INITIATE");
                    client.Network.Logout();
                }
            }
            else
            {
                MatingRitualLogger.Log("CLIENT", "LOGIN", "FAIL", client.Network.LoginMessage);
            }
        }
    }
}
