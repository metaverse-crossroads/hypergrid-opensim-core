using System;
using OpenMetaverse;
using System.Threading;
using log4net.Config;
using log4net;
using System.Reflection;

namespace OmvTestHarness
{
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

            Console.WriteLine($"Attempting to login to {loginURI} as {firstName} {lastName}...");

            GridClient client = new GridClient();

            // Register callbacks
            client.Network.LoginProgress += (sender, e) =>
            {
                Console.WriteLine($"Login Progress: {e.Status} - {e.Message}");
            };

            LoginParams loginParams = client.Network.DefaultLoginParams(firstName, lastName, password, "OmvTestHarness", "1.0.0");
            loginParams.URI = loginURI;

            if (client.Network.Login(loginParams))
            {
                Console.WriteLine("Login Successful!");
                Console.WriteLine($"Agent ID: {client.Self.AgentID}");
                Console.WriteLine($"Session ID: {client.Self.SessionID}");
                Console.WriteLine($"Sim Name: {client.Network.CurrentSim.Name}");

                // Stay connected for a bit
                Thread.Sleep(5000);

                client.Network.Logout();
                Console.WriteLine("Logged out.");
            }
            else
            {
                Console.WriteLine($"Login Failed: {client.Network.LoginMessage}");
            }
        }
    }
}
