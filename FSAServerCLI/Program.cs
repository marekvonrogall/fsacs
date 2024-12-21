using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FSAServerCLI.Classes;
using WebSocketSharp.Server;

namespace FSAServerCLI
{
    internal class Program
    {
        public static List<AvailableClient> _availableClients = new List<AvailableClient>();
        public static List<RegisteredClientDetails> _registeredClients = new List<RegisteredClientDetails>();
        public static int _nextClientId = 1;
        private static NetworkInterfaces networkInterfaces = new NetworkInterfaces();
        private static PortServices portServices = new PortServices();

        static void Main(string[] args)
        {
            while (true)
            {
                networkInterfaces.ListNetworkInterfaces();

                Console.WriteLine("Select on which network you want to host your server:");
                string networkSelection = Console.ReadLine();
                NetworkInterfaces selectedNetworkInterface = networkInterfaces.AllNetworkInterfaces[int.Parse(networkSelection) - 1];
                IPAddress ipAddress = selectedNetworkInterface.Address;
                string hostAddress = ipAddress + $":{portServices.GetFreeTcpPort(ipAddress)}";

                try
                {
                    var ws = new WebSocketServer($"ws://{hostAddress}");
                    ws.AddWebSocketService<FSAServerBehavior>("/FSAServer");
                    ws.Start();

                    string webSocketAddress = $"ws://{hostAddress}/FSAServer";
                    Console.WriteLine($"Server started at {webSocketAddress}");

                    Task.Run(() => UDPBroadcast.StartUdpBroadcastServer(selectedNetworkInterface, webSocketAddress));

                    Console.ReadLine();
                    ws.Stop();
                    Environment.Exit(0);
                }
                catch(Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }
            }
            
        }
    }

    public record RegisterData(string Name, string IpAddress, int Port);
    public record RequestData(int UserId, int SenderId, string FileName, string FileSize);
    public record RegisteredClientDetails(int Id, string Name, string IpAddress, int Port);
    public record P2PConnectionData(int SenderId, int ReceiverId, string answer);
    public record RequestResponse(string Type, string IPAddress, int Port);
}
