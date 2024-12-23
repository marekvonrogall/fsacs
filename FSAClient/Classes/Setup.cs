using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FSAClient.Classes
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup
    {
        private PortServices portService = new PortServices();
        private NetworkInterfaces networkInterfaces = new NetworkInterfaces();
        UDPBroadcast udpBroadcast = new UDPBroadcast();

        public Label labelStatusMessage { get; set; }
        public ComboBox comboBoxNetworkInterfaces { get; set; }

        public async Task PopulateNetworkInterfaces()
        {
            comboBoxNetworkInterfaces.IsEnabled = false;
            networkInterfaces.PopulateNetworkInterfaces(comboBoxNetworkInterfaces);
            await ValidateNetworkInterfaces(comboBoxNetworkInterfaces.Items);
        }

        public async Task CheckValidNetworkInterfaces(Button buttonRetry, Button buttonManualSetup)
        {
            switch (comboBoxNetworkInterfaces.Items.Count)
            {
                case 0: // NO SERVERS FOUND --> ENABLE MANUAL SETUP
                    labelStatusMessage.Content = "FSA konnte keinen Server in ihrem Netzwerk finden.";
                    buttonRetry.IsEnabled = true;
                    buttonManualSetup.IsEnabled = true;
                    break;
                case 1: // ONE SERVER FOUND
                    labelStatusMessage.Content = "Ein Server wurde in ihrem Netzwerk gefunden.";
                    MainWindow.Instance.NavigateToPage(new Username(this));

                    break;
                default: //MULTIPLE SERVERS FOUND
                    labelStatusMessage.Content = "Es wurden mehrere Server in ihren Netzwerken gefunden!";
                    comboBoxNetworkInterfaces.SelectedIndex = 0;
                    comboBoxNetworkInterfaces.IsEnabled = true;
                    // MÖGLICHKEIT ButtonFinishSetup_Click aus AUTOMATED SETUP AUSZUFÜHREN!
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
                comboBoxNetworkInterfaces.Items.Remove(item);
            }
        }

        private async Task SetClientInfo(IPAddress clientIp, string username)
        {
            int clientTCPPort = portService.GetFreeTcpPort(clientIp);
            if (clientTCPPort != 0)
            {
                UserData.StoreUserData(username, clientIp, clientTCPPort);
            }
        }

        public async Task FinishSetup(string username)
        {
            ComboBoxItem availableNetworkInterfaceItem = (ComboBoxItem)comboBoxNetworkInterfaces.Items[0];
            NetworkInterfaces availableNetworkInterface = (NetworkInterfaces)availableNetworkInterfaceItem.Tag;

            await SetClientInfo(availableNetworkInterface.Address, username);

            if (UserData.IsValid)
            {
                MainWindow.Instance.NavigateToPage(new FSA(availableNetworkInterface.WebSocketAddress));
            }
            else MessageBox.Show("Ein Fehler ist aufgetreten.");
        }
    }
}
