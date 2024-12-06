using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for ConnectionAlert.xaml
    /// </summary>
    public partial class ConnectionAlert : Window
    {
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
