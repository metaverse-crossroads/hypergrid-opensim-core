using System;
using OpenMetaverse;
using System.Threading;

namespace OmvTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            string firstName = "Test";
            string lastName = "User";
            string password = "1234";
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
