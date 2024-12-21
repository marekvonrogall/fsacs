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
        private IPServices iPService = new IPServices();
        private PortServices portService = new PortServices();
        private NetworkInterfaces networkInterfaces = new NetworkInterfaces();

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
            if (CheckBoxLocalMode.IsChecked == true)
            {
                TextBoxPublicIP.Text = TextBoxLocalIP.Text;
                TextBoxPublicPort.Text = TextBoxLocalPort.Text;
            }

            try
            {
                if (UserData.StoreUserData(TextBoxName.Text, TextBoxLocalIP.Text, TextBoxLocalPort.Text,
                        TextBoxPublicIP.Text, TextBoxPublicPort.Text))
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

        private void CheckBoxLocalMode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxPublicIP.IsEnabled = false;
            TextBoxPublicPort.IsEnabled = false;
        }

        private void CheckBoxLocalMode_Unchecked(object sender, RoutedEventArgs e)
        {
            TextBoxPublicIP.IsEnabled = true;
            TextBoxPublicPort.IsEnabled = true;
        }
    }
}
