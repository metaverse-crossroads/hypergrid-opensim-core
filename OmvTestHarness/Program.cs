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

            MatingRitualLogger.Log("CLIENT", "LOGIN", "START", $"URI: {loginURI}, User: {firstName} {lastName}");

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

            if (client.Network.Login(loginParams))
            {
                MatingRitualLogger.Log("CLIENT", "LOGIN", "SUCCESS", $"Agent: {client.Self.AgentID}");

                // Stay connected for a bit
                Thread.Sleep(5000);

                MatingRitualLogger.Log("CLIENT", "LOGOUT", "INITIATE");
                client.Network.Logout();
            }
            else
            {
                MatingRitualLogger.Log("CLIENT", "LOGIN", "FAIL", client.Network.LoginMessage);
            }
        }
    }
}
