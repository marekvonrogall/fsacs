using System;
using System.Collections.Generic;   
using System.Text.Json;
using System.Windows;

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

        //establish connection to server

        public void RegisterClient()
        {
            //WEBSOCKET TRANSMIT DATA AS STRING
            RegisterData registerData = new RegisterData(UserData.Name, UserData.PublicIP.ToString(), UserData.PublicPort);
            string serializedClient = JsonSerializer.Serialize(registerData);
            string message = $"clientRegistration;{serializedClient}";

            //SEND MESSAGE TO SERVER
        }

        public void RetrieveAvailableClients()
        {
            //SAMPLE DATA: ACTUALLY RETREIVE DATA FROM SERVER HERE
            AvailableClients = new List<AvailableClient>
            {
                new AvailableClient(232311, "Marek's Desktop"),
                new AvailableClient(144432, "Pascal"),
                new AvailableClient(847319, "Stefan H. Jesenko")
            };

            if(UserData.UserId != 0)
            {
                AvailableClients = AvailableClients.FindAll(client => client.Id != UserData.UserId);
            }
        }
    }
}
