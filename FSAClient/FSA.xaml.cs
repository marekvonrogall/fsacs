using FSAClient.Classes;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WebSocketSharp;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for FSA.xaml
    /// </summary>
    public partial class FSA : Page
    {
        private ServerCommunication serverCommunication;
       
        
        private Client client = new Client();

        public AvailableClient selectedClient;
        WebSocket ws;

        public FSA(string externalServerAddress)
        {
            InitializeComponent();

            serverCommunication = new ServerCommunication(externalServerAddress, ws);
            serverCommunication.RegisterClient();

            PopulateClientList();
        }

        private void PopulateClientList()
        {
            ClientsListBox.ItemsSource = serverCommunication.AvailableClients;
        }
        record RequestData (int UserId, int SenderId, string FileName, string FileSize);
        private void ButtonEstablishConnection_Click(object sender, RoutedEventArgs e)
        {
            client.SelectFile();    
            RequestData request = new RequestData(UserData.UserId, selectedClient.Id, client.FileName, client.FileSize);
            string serializedrequest = JsonSerializer.Serialize(request);
            string message = $"FileSendRequest;{serializedrequest}";
            
          
            //FOR TESTING PURPOSES ONLY
            IPAddress serverResponsePublicIP = UserData.PublicIP;
            int serverResponsePublicPort = UserData.PublicPort;

            client.SendData(serverResponsePublicPort, serverResponsePublicIP);
        }

       

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClient = ClientsListBox.SelectedItem as AvailableClient;
            if (selectedClient != null)
            {
                LabelSelectedClientName.Content = "Selected Client: " + selectedClient.Name;
                LabelSelectedClientID.Content = "ID: " + selectedClient.Id;
            }
        }
    }
}
