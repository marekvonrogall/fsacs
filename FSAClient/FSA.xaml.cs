﻿using FSAClient.Classes;
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
        record RequestData(int UserId, int SenderId, string FileName, string FileSize);
        private ServerCommunication serverCommunication;
        private Client client = new Client();
        public AvailableClient selectedClient;
        WebSocket ws;

        public FSA(string webSocketAddress)
        {
            InitializeComponent();
            EstablishWebsocketConnection(webSocketAddress);
        }

        private async void EstablishWebsocketConnection(string webSocketAddress)
        {
            ws = new WebSocket(webSocketAddress);
            serverCommunication = new ServerCommunication(ws, client, this);
            serverCommunication.RegisterClient();
        }

        public void PopulateClientList()
        {
            ClientsListBox.Dispatcher.Invoke(() => //Dispatcher.Invoke: Vorschlag von ChatGPT (Behebung Fehler: "owned by a different thread")
            {
                ClientsListBox.ItemsSource = serverCommunication.AvailableClients;
            });
        }

        public void UpdateUserID()
        {
            LabelThisClientInfo.Dispatcher.Invoke(() => //Dispatcher.Invoke: Vorschlag von ChatGPT (Behebung Fehler: "owned by a different thread")
            {
                LabelThisClientInfo.Content = $"Name: {UserData.Name} - ID: {UserData.UserId}";
            });
        }

        private void ButtonEstablishConnection_Click(object sender, RoutedEventArgs e)
        {
            client.FileName = null;
            client.SelectFile();
            if (client.FileName != null)
            {
                RequestData request = new RequestData(UserData.UserId, selectedClient.Id, client.FileName, client.FileSize);
                string serializedRequest = JsonSerializer.Serialize(request);
                string message = $"FileSendRequest;{serializedRequest}";
                ws.Send(message);
            }
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
