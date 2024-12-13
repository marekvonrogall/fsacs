using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace FSASampleServer
{
    internal class Program
    {
        public static List<AvailableClient> _availableClients = new List<AvailableClient>
        {
            new AvailableClient(1, "Pascal"),
            new AvailableClient(2, "Marek"),
            new AvailableClient(3, "Stefan"),
            new AvailableClient(4, "Cyril"),
            new AvailableClient(5, "Manuel")

        };

        static void Main(string[] args)
        {
            var ws = new WebSocketServer("ws://localhost:5000");
            ws.AddWebSocketService<FSAServerBehavior>("/FSAServer");
            ws.Start();
            Console.WriteLine("Server started at ws://localhost:5000/FSAServer");
            Console.ReadLine();
            ws.Stop();
        }
    }

    public class FSAServerBehavior : WebSocketBehavior
    {
        private static List<FSAServerBehavior> _connectedClients = new List<FSAServerBehavior>();

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
                    Console.WriteLine($"Client registered: {registerData.Name}, {registerData.IpAddress}, {registerData.Port}");

                    // Send ClientId to the registering client
                    string clientIdMessage = "ClientId;2";
                    Send(clientIdMessage);
                    Console.WriteLine($"Sent message to client: {clientIdMessage}");

                    // Send available clients to all connected clients
                    string availableClientsMessage = "AvailableClients;" + JsonSerializer.Serialize(Program._availableClients);
                    foreach (var client in _connectedClients)
                    {
                        client.Send(availableClientsMessage);
                        Console.WriteLine($"Sent message to client: {availableClientsMessage}");
                    }
                    break;
                case "FileSendRequest":
                    RequestData fileSendRequest = JsonSerializer.Deserialize<RequestData>(message[1]);
                    //...
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _connectedClients.Remove(this);
            Console.WriteLine("Client disconnected.");
        }
    }

    public record RegisterData(string Name, string IpAddress, int Port);
        record RequestData(int UserId, int SenderId, string FileName, string FileSize);

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
