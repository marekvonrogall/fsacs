using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace FSAServerCLI
{
    internal class Program
    {
        public static List<AvailableClient> _availableClients = new List<AvailableClient>();
        public static List<RegisteredClientDetails> _registeredClients = new List<RegisteredClientDetails>();
        public static int _nextClientId = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the host-address and port \"(IP:Port)\": ");
            string hostAddress = Console.ReadLine();

            var ws = new WebSocketServer($"ws://{hostAddress}");
            ws.AddWebSocketService<FSAServerBehavior>("/FSAServer");
            ws.Start();

            string webSocketAddress = $"ws://{hostAddress}/FSAServer";
            Console.WriteLine($"Server started at {webSocketAddress}");

            Task.Run(() => StartUdpBroadcastServer(webSocketAddress));

            Console.ReadLine();
            ws.Stop();
        }

        private static async void StartUdpBroadcastServer(string responseMessage)
        {
            Console.WriteLine("UDP Server: Listening for UDP discovery requests...");
            
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 17550);
            UdpClient listener = new UdpClient(17551);

            while (true)
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    listener.EnableBroadcast = true;

                    var receivedData = await listener.ReceiveAsync();

                    if (Encoding.UTF8.GetString(receivedData.Buffer) == "DISCOVER_WEBSOCKET_SERVER")
                    {
                        byte[] data = Encoding.UTF8.GetBytes(responseMessage);
                        await udpClient.SendAsync(data, data.Length, endPoint);
                        Console.WriteLine("Received UDP Request, sent back WebSocket-Address!");
                    }
                }
            }
        }
    }

    public class FSAServerBehavior : WebSocketBehavior
    {
        private static List<FSAServerBehavior> _connectedClients = new List<FSAServerBehavior>();
        private int _clientId;

        protected override void OnOpen()
        {
            _connectedClients.Add(this);
            Console.WriteLine("New client connected.");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine($"Received message: {e.Data}");
            string[] message = e.Data.Split(';');

            switch (message[0])
            {
                case "ClientRegistration":
                    RegisterData registerData = JsonSerializer.Deserialize<RegisterData>(message[1]);
                    _clientId = Program._nextClientId++;
                    var clientDetails = new RegisteredClientDetails(_clientId, registerData.Name, registerData.IpAddress, registerData.Port);
                    Program._registeredClients.Add(clientDetails);

                    // Send ClientId to the registering client
                    string clientIdMessage = $"ClientId;{_clientId}";
                    Send(clientIdMessage);
                    Console.WriteLine($"Sent message to client: {clientIdMessage}");

                    UpdateAvailableClients();

                    break;
                case "FileSendRequest":
                    RequestData fileSendRequest = JsonSerializer.Deserialize<RequestData>(message[1]);
                    ConnectionAlertData connectionAlertData = new ConnectionAlertData
                    {
                        UserName = Program._registeredClients.First(client => client.Id == fileSendRequest.UserId).Name,
                        UserId = fileSendRequest.UserId,
                        IpAddress = Program._registeredClients.First(client => client.Id == fileSendRequest.UserId).IpAddress,
                        Port = Program._registeredClients.First(client => client.Id == fileSendRequest.UserId).Port,
                        FileName = fileSendRequest.FileName,
                        FileSize = fileSendRequest.FileSize
                    };
                    string connectionAlertMessage = $"IncomingRequest;{JsonSerializer.Serialize(connectionAlertData)}";
                    foreach (var client in _connectedClients)
                    {
                        if (client._clientId == fileSendRequest.SenderId)
                        {
                            client.Send(connectionAlertMessage);
                            Console.WriteLine($"Sent message to client: {connectionAlertMessage}");
                            break;
                        }
                    }
                    break;
                case "P2PConnectionResponse":
                    P2PConnectionData p2pConnectionData = JsonSerializer.Deserialize<P2PConnectionData>(message[1]);

                    RequestResponse requestResponse;
                    if (p2pConnectionData.answer == "accept")
                    {
                        requestResponse = new RequestResponse(p2pConnectionData.answer,
                            Program._registeredClients.First(client => client.Id == p2pConnectionData.SenderId).IpAddress,
                            Program._registeredClients.First(client => client.Id == p2pConnectionData.SenderId).Port);
                    }
                    else requestResponse = new RequestResponse(p2pConnectionData.answer, null, 0);

                    string p2pRequestResponse = "RequestResponse;" + JsonSerializer.Serialize(requestResponse);

                    foreach (var client in _connectedClients)
                    {
                        if (client._clientId == p2pConnectionData.ReceiverId)
                        {
                            client.Send(p2pRequestResponse);
                            Console.WriteLine($"Sent message to client: {p2pRequestResponse}");
                            break;
                        }
                    }
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _connectedClients.Remove(this);
            Program._registeredClients.RemoveAll(client => client.Id == _clientId);
            UpdateAvailableClients();
            Console.WriteLine("Client disconnected.");
        }

        private void UpdateAvailableClients()
        {
            Program._availableClients = Program._registeredClients
                .Select(client => new AvailableClient(client.Id, client.Name))
                .ToList();

            // Send available clients to all connected clients
            string availableClientsMessage = "AvailableClients;" + JsonSerializer.Serialize(Program._availableClients);
            foreach (var client in _connectedClients)
            {
                client.Send(availableClientsMessage);
                Console.WriteLine($"Sent message to client: {availableClientsMessage}");
            }
        }
    }

    public record RegisterData(string Name, string IpAddress, int Port);
    public record RequestData(int UserId, int SenderId, string FileName, string FileSize);
    public record RegisteredClientDetails(int Id, string Name, string IpAddress, int Port);
    public record P2PConnectionData(int SenderId, int ReceiverId, string answer);
    public record RequestResponse(string Type, string IPAddress, int Port);

    public class AvailableClient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AvailableClient(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ConnectionAlertData
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
    }
}
