using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for ConnectionAlert.xaml
    /// </summary>
    public partial class ConnectionAlert : Window
    {
        public class ConnectionAlertData
        {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public string IpAddress { get; set; }
            public int Port { get; set; }
            public string FileName { get; set; }
            public string FileSize { get; set; }
        }
        ConnectionAlertData IncomingConnection;
        public ConnectionAlert(string serializedConnectionAlert)
        {
            InitializeComponent();
            ButtonAcceptConnection.IsEnabled = false;
            IncomingConnection = JsonSerializer.Deserialize<ConnectionAlertData>(serializedConnectionAlert);
            this.DataContext = IncomingConnection;
        }

        private void CheckBoxTrustRequest_Checked(object sender, RoutedEventArgs e)
        {
            ButtonAcceptConnection.IsEnabled = true;
            ButtonAcceptConnection.Background = new SolidColorBrush(Color.FromRgb(173, 208, 170));
        }

        private void CheckBoxTrustRequest_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonAcceptConnection.IsEnabled = false;
            ButtonAcceptConnection.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        }

        private void ButtonDeclineConnection_Click(object sender, RoutedEventArgs e)
        {
            //DECLINE
        }

        private void ButtonAcceptConnection_Click(object sender, RoutedEventArgs e)
        {
            //ACCEPT
        }
    }
}
