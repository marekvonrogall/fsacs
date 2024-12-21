﻿using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Windows;
using WebSocketSharp;

namespace FSAClient.Classes
{
    public class ServerCommunication
    {
        public List<AvailableClient> AvailableClients { get; private set; }
        public record RegisterData(string Name, string IpAddress, int Port);
        private WebSocket ws;
        private Client _client;
        private FSA _fsa;

        public ServerCommunication(WebSocket webSocket, Client client, FSA fsa)
        {
            _fsa = fsa;
            _client = client;
            ws = webSocket;
            ws.OnMessage += Ws_OnMessage;
        }

        public void RegisterClient()
        {
            ws.Connect();
            RegisterData registerData = new RegisterData(UserData.Name, UserData.LocalIP.ToString(), UserData.LocalPort);
            string serializedClient = JsonSerializer.Serialize(registerData);
            string message = $"ClientRegistration;{serializedClient}";
            ws.Send(message);
        }

        public void RetrieveAvailableClients(string serializedClients)
        {
            AvailableClients = JsonSerializer.Deserialize<List<AvailableClient>>(serializedClients);

            if (UserData.UserId != 0)
            {
                AvailableClients = AvailableClients.FindAll(client => client.Id != UserData.UserId);
            }

            _fsa.PopulateClientList();
        }

        record RequestResponse(string Type, string IPAddress, int Port);

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            string[] message = e.Data.Split(';');
            switch (message[0])
            {
                case "ClientId":
                    UserData.UserId = int.Parse(message[1]);
                    _fsa.UpdateUserID();
                    break;
                case "AvailableClients":
                    RetrieveAvailableClients(message[1]);
                    break;
                case "IncomingRequest":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ConnectionAlert incomingRequest = new ConnectionAlert(message[1], ws);
                        incomingRequest.Show();
                    });
                    break;
                case "RequestResponse":
                    RequestResponse requestResponse = JsonSerializer.Deserialize<RequestResponse>(message[1]);
                    if (requestResponse.Type == "accept") _client.SendData(requestResponse.Port, IPAddress.Parse(requestResponse.IPAddress));
                    else MessageBox.Show("Ihre Anfrage wurde abgelehnt!");
                    break;
            }
        }
    }
}
