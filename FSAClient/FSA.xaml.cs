using FSAClient.Classes;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for FSA.xaml
    /// </summary>
    public partial class FSA : Page
    {
        private IPServices iPServices = new IPServices();
        private Listener listener = new Listener();
        private Client client = new Client();

        public record AvailableClient(int Id, string Name);
        public AvailableClient selectedClient;
        public List<AvailableClient> AvailableClients;

        public FSA()
        {
            InitializeComponent();
            AvailableClients = GetAvailableClients();
            ClientsListBox.ItemsSource = AvailableClients;
        }

        private void ButtonEstablishConnection_Click(object sender, RoutedEventArgs e)
        {
            //SEND selectedClient TO SERVER

            //GET RESPONSE FROM SERVER WITH IP_ADDRESS:PORT OF selectedClient

            //FOR TESTING PURPOSES ONLY
            IPAddress serverResponsePublicIP = UserData.PublicIP;
            int serverResponsePublicPort = UserData.PublicPort;

            client.SendData(serverResponsePublicPort, serverResponsePublicIP);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listener.Listen();
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

        

        
        private List<AvailableClient> GetAvailableClients()
        {
            //ACTUALLY RETRIEVE CLIENT LIST FROM SERVER HERE AS "LIST<AVAILABLECLIENT>"

            //FOR TESTING PURPOSES GENERATED CLIENTS:
            List<AvailableClient> clients = new List<AvailableClient>
            {
                new AvailableClient(232311, "Marek's Desktop"),
                new AvailableClient(144432, "Pascal"),
                new AvailableClient(847319, "Stefan H. Jesenko")
            };

            return clients;
        }
    }
}
