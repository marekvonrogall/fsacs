using System;
using System.Collections.Generic;   
using System.Text.Json;
using System.Windows;
using System.Windows.Markup;
using WebSocketSharp;


namespace FSAClient.Classes
{
    public class ServerCommunication
    {

      
        public List<AvailableClient> AvailableClients { get; private set; }
        private string ExternalServerAddress { get; set; }

        public record RegisterData(string Name, string IpAddress, int Port);

        public ServerCommunication(string externalServerAddress)
        {
            ExternalServerAddress = externalServerAddress;
        }
        WebSocket ws;


        //establish connection to server

        public void RegisterClient()
        {
            //WEBSOCKET TRANSMIT DATA AS STRING
            ws = new WebSocket(ExternalServerAddress);
            ws.Connect();   




            RegisterData registerData = new RegisterData(UserData.Name, UserData.PublicIP.ToString(), UserData.PublicPort);
            string serializedClient = JsonSerializer.Serialize(registerData);
            string message = $"clientRegistration;{serializedClient}";
            ws.Send(message);

            //SEND MESSAGE TO SERVER
        }

        public void RetrieveAvailableClients(string serializedClients)
        {
            AvailableClients = JsonSerializer.Deserialize<List<AvailableClient>>(serializedClients);



            if (UserData.UserId != 0)
            {
                AvailableClients = AvailableClients.FindAll(client => client.Id != UserData.UserId);
            }
        }

        private  void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            string[] message = e.Data.Split(';');
            switch (message[0])
            {
                case "clientId":
                    UserData.UserId = int.Parse(message[1]);
                    break;
                case "availableClients":
                    RetrieveAvailableClients(message[1]);
                    break;
                case "IncomingRequest":

                    break;
                case "RequestResponse":

                    break;
            }
        }

    }
}
