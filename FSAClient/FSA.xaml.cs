using FSAClient.Classes;
using System.Collections.Generic;
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
        private ServerCommunication serverCommunication;
        private IPServices iPServices = new IPServices();
        private Listener listener = new Listener();
        private Client client = new Client();

        public AvailableClient selectedClient;
        public List<AvailableClient> AvailableClients;

        public FSA(string externalServerAddress)
        {
            InitializeComponent();

            serverCommunication = new ServerCommunication(externalServerAddress);
            serverCommunication.RegisterClient();

            PopulateClientList();
        }

        private void PopulateClientList()
        {
            AvailableClients = serverCommunication.RetreiveClients();
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
    }
}
