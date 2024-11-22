using FSAClient.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            IPAddress serverResponsePublicIP = IPAddress.Parse("192.168.18.4");
            int serverResponsePublicPort = 5030;

            client.EstablishConnection(serverResponsePublicPort, serverResponsePublicIP);
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
