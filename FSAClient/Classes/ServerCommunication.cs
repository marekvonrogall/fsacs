using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FSAClient.Classes
{
    public class ServerCommunication
    {
        private string ExternalServerAddress { get; set; }
        public record RegisterData(string Name, string IpAddress, int Port);

        public ServerCommunication(string externalServerAddress)
        {
            ExternalServerAddress = externalServerAddress;
        }

        public int RegisterClient()
        {
            //WEBSOCKET TRANSMIT DATA AS STRING
            RegisterData registerData = new RegisterData(UserData.Name, UserData.PublicIP.ToString(), UserData.PublicPort);
            string serializedClient = JsonSerializer.Serialize(registerData);

            //WEBSOCKET RETURNS USER ID
            int userId = 1;

            return userId;
        }
    }
}
