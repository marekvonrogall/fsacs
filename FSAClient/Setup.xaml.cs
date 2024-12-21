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

        public Setup()
        {
            InitializeComponent();
            PopulateNetworkInterfaces();
        }

        private async void PopulateNetworkInterfaces()
        {
            ComboBoxNetworkInterfaces.IsEnabled = false;
            networkInterfaces.PopulateNetworkInterfaces(ComboBoxNetworkInterfaces);
            await ValidateNetworkInterfaces(ComboBoxNetworkInterfaces.Items);

            if (ComboBoxNetworkInterfaces.Items.Count != 0)
            {
                ComboBoxNetworkInterfaces.SelectedIndex = 0;
                ComboBoxNetworkInterfaces.IsEnabled = true;
                LabelNetworkInterfaceStatus.Content = "Server found!";
            }
            else
            {
                LabelNetworkInterfaceStatus.Content = "There is no server running on your available networks...";
            }
        }

        private async Task ValidateNetworkInterfaces(ItemCollection networkInterfaces)
        {
            List<ComboBoxItem> itemsToRemove = new List<ComboBoxItem>();
            foreach (ComboBoxItem networkInterface in networkInterfaces)
            {
                NetworkInterfaces ni = (NetworkInterfaces)networkInterface.Tag;
                LabelNetworkInterfaceStatus.Content = $"Checking {ni.Name} - {ni.Description}...";

                string serverAddress = await udpBroadcast.LookForServerAsync(ni.Address);
                int freeTCPPort = portService.GetFreeTcpPort(ni.Address);

                if (serverAddress == "NO_SERVER_FOUND" || freeTCPPort == 0)
                {
                    itemsToRemove.Add(networkInterface);
                }
            }
            foreach (var item in itemsToRemove)
            {
                ComboBoxNetworkInterfaces.Items.Remove(item);
            }
        }

        private async void CheckForWebsocket(IPAddress localAddress)
        {
            string serverAddress = await udpBroadcast.LookForServerAsync(localAddress);
            if (serverAddress != "NO_SERVER_FOUND")
            {
                TextBoxServerAddress.Text = serverAddress;
            }
        }

        private void NetworkInterfaceChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)ComboBoxNetworkInterfaces.SelectedItem;
            NetworkInterfaces ni = (NetworkInterfaces)selectedItem.Tag;
            CheckForWebsocket(ni.Address);
            SetClientInfo(ni.Address);
        }

        private void SetClientInfo(IPAddress clientIp)
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
                var newPage = new FSA(TextBoxServerAddress.Text);
                MainWindow.Instance.NavigateToPage(newPage);
            }
        }
    }
}
