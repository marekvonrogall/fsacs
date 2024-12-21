using System;
using System.Net;
using System.Net.NetworkInformation;
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
        private bool localMode = true;

        public Setup()
        {
            InitializeComponent();
            networkInterfaces.PopulateNetworkInterfaces(ComboBoxNetworkInterfaces);
            if (ComboBoxNetworkInterfaces.Items.Count != 0)
            {
                ComboBoxNetworkInterfaces.SelectedIndex = 0;
            }
        }

        private void NetworkInterfaceChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)ComboBoxNetworkInterfaces.SelectedItem;
            NetworkInterfaces ni = (NetworkInterfaces)selectedItem.Tag;
            GetClientInfo(ni.Address);
        }

        private async void GetClientInfo(IPAddress ip)
        {
            try
            {
                TextBoxLocalIP.Text = ip.ToString();
                TextBoxLocalPort.Text = portService.GetFreeTcpPort(ip).ToString();
            }
            catch { }
        }

        private void ButtonFinishSetup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserData.StoreUserData(TextBoxName.Text, TextBoxLocalIP.Text, TextBoxLocalPort.Text))
                {
                    var newPage = new FSA(TextBoxServerAddress.Text);
                    MainWindow.Instance.NavigateToPage(newPage);
                }
                else throw new Exception();
            }
            catch
            {
                MessageBox.Show("Ungültige Eingaben erkannt.", "Error");
            }
        }
    }
}
