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
        public record RegisterData(string Name, string IpAddress, int Port);
        WebSocket ws;
        public ServerCommunication(string externalServerAddress, WebSocket webSocket)
        {
            
            webSocket = new WebSocket(externalServerAddress);
            ws = webSocket;
        }
        


        //establish connection to server

        public void RegisterClient()
        {
           
            
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
                    ConnectionAlert incomingRequest = new ConnectionAlert(message[1], ws);
                    incomingRequest.Show();
                    break;
                
            }
        }

    }
}
