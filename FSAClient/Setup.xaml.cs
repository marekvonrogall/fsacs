using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        private PortServices portService = new PortServices();
        private NetworkInterfaces networkInterfaces = new NetworkInterfaces();
        UDPBroadcast udpBroadcast = new UDPBroadcast();

        public Label labelStatusMessage { get; set; }

        public Setup()
        {
            //InitializeComponent();
        }

        public async Task PopulateNetworkInterfaces()
        {
            InitializeComponent();
            ComboBoxNetworkInterfaces.IsEnabled = false;
            networkInterfaces.PopulateNetworkInterfaces(ComboBoxNetworkInterfaces);
            await ValidateNetworkInterfaces(ComboBoxNetworkInterfaces.Items);
        }

        public async Task CheckValidNetworkInterfaces(Button buttonRetry, Button buttonManualSetup)
        {
            switch (ComboBoxNetworkInterfaces.Items.Count)
            {
                case 0: // NO SERVERS FOUND
                    labelStatusMessage.Content = "FSA konnte keinen Server in ihrem Netzwerk finden.";
                    buttonRetry.IsEnabled = true;
                    buttonManualSetup.IsEnabled = true;
                    break;
                case 1: // ONE SERVER FOUND
                    labelStatusMessage.Content = "Ein Server wurde in ihrem Netzwerk gefunden.";

                    ComboBoxItem availableNetworkInterfaceItem = (ComboBoxItem)ComboBoxNetworkInterfaces.Items[0];
                    NetworkInterfaces availableNetworkInterface = (NetworkInterfaces)availableNetworkInterfaceItem.Tag;

                    await SetClientInfo(availableNetworkInterface.Address);

                    var newPage = new FSA(availableNetworkInterface.WebSocketAddress);
                    MainWindow.Instance.NavigateToPage(newPage);
                    break;
                default: //MULTIPLE SERVERS FOUND
                    labelStatusMessage.Content = "Es wurden mehrere Server in ihren Netzwerken gefunden!";
                    ComboBoxNetworkInterfaces.SelectedIndex = 0;
                    ComboBoxNetworkInterfaces.IsEnabled = true;
                    LabelNetworkInterfaceStatus.Content = "Server found!";
                    break;
            }
        }

        private async Task ValidateNetworkInterfaces(ItemCollection networkInterfaces)
        {
            List<ComboBoxItem> itemsToRemove = new List<ComboBoxItem>();
            foreach (ComboBoxItem networkInterface in networkInterfaces)
            {
                NetworkInterfaces ni = (NetworkInterfaces)networkInterface.Tag;

                labelStatusMessage.Content = $"Checking Network Interface {ni.Name} - {ni.Description}...";

                string serverAddress = await udpBroadcast.LookForServerAsync(ni);
                int freeTCPPort = portService.GetFreeTcpPort(ni.Address);

                if (serverAddress == "NO_SERVER_FOUND" || freeTCPPort == 0)
                {
                    itemsToRemove.Add(networkInterface);
                }
                else
                {
                    ni.WebSocketAddress = serverAddress;
                }
            }
            foreach (var item in itemsToRemove)
            {
                ComboBoxNetworkInterfaces.Items.Remove(item);
            }
        }

        private async Task SetClientInfo(IPAddress clientIp)
        {
            ButtonFinishSetup.IsEnabled = false;
            int clientTCPPort = portService.GetFreeTcpPort(clientIp);
            if (clientTCPPort != 0)
            {
                UserData.StoreUserData(TextBoxName.Text, clientIp, clientTCPPort);

                if (UserData.IsValid)
                {
                    TextBoxLocalIP.Text = UserData.LocalIP.ToString();
                    TextBoxLocalPort.Text = UserData.LocalPort.ToString();
                    ButtonFinishSetup.IsEnabled = true;
                }
                else
                {
                    TextBoxLocalIP.Text = string.Empty;
                    TextBoxLocalPort.Text = string.Empty;
                    MessageBox.Show("Try using a different network interface.", "Network Interface Error");
                }
            }
        }

        private void ButtonFinishSetup_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.IsValid)
            {
                ComboBoxItem selectedNetworkInterfaceItem = (ComboBoxItem)ComboBoxNetworkInterfaces.SelectionBoxItem;
                NetworkInterfaces selectedNetworkInterface = (NetworkInterfaces)selectedNetworkInterfaceItem.Tag;

                SetClientInfo(selectedNetworkInterface.Address);

                var newPage = new FSA(selectedNetworkInterface.WebSocketAddress);
                MainWindow.Instance.NavigateToPage(newPage);
            }
        }
    }
}
