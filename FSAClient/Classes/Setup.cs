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

        public async Task CheckValidNetworkInterfaces(Button buttonRetry, Button buttonManualSetup, Button buttonUseNetwork)
        {
            buttonRetry.IsEnabled = true;
            buttonManualSetup.IsEnabled = true;

            switch (comboBoxNetworkInterfaces.Items.Count)
            {
                case 0: // NO SERVERS FOUND --> ENABLE MANUAL SETUP
                    labelStatusMessage.Content = "Es konnten keine Server gefunden werden.";
                    break;
                /*
                case 1: // ONE SERVER FOUND
                    labelStatusMessage.Content = "Ein Server wurde in ihrem Netzwerk gefunden.";
                    comboBoxNetworkInterfaces.SelectedIndex = 0;
                    MainWindow.Instance.NavigateToPage(new Username(this));
                    break;*/
                default: //AT LEAST 1 SERVER HAS BEEN FOUND
                    labelStatusMessage.Content = "FSAServer gefunden!";
                    comboBoxNetworkInterfaces.SelectedIndex = 0;
                    comboBoxNetworkInterfaces.IsEnabled = true;
                    buttonUseNetwork.IsEnabled = true;
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
            try
            {
                ComboBoxItem selectedNetworkInterfaceItem = (ComboBoxItem)comboBoxNetworkInterfaces.SelectedItem;
                NetworkInterfaces selectedNetworkInterface = (NetworkInterfaces)selectedNetworkInterfaceItem.Tag;

                await SetClientInfo(selectedNetworkInterface.Address, username);

                if (UserData.IsValid)
                {
                    MainWindow.Instance.NavigateToPage(new FSA(selectedNetworkInterface.WebSocketAddress));
                }
                else throw new Exception();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
