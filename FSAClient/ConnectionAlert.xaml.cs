using System.Net.WebSockets;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using FSAClient.Classes;
using WebSocketSharp;   
namespace FSAClient
{
    /// <summary>
    /// Interaction logic for ConnectionAlert.xaml
    /// </summary>
    public partial class ConnectionAlert : Window
    {
        ConnectionAlertData IncomingConnection;
        WebSocketSharp.WebSocket webSocket;
        
        public ConnectionAlert(string serializedConnectionAlert, WebSocketSharp.WebSocket ws)
        {
            InitializeComponent();
            ButtonAcceptConnection.IsEnabled = false;
            IncomingConnection = JsonSerializer.Deserialize<ConnectionAlertData>(serializedConnectionAlert);
            this.DataContext = IncomingConnection;
            webSocket = ws;
        }
        record P2PConnectionData(int SenderId, int ReceiverId, string answer);
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
            Listener listener = new Listener();
            P2PConnectionData p2PConnectionData = new P2PConnectionData(UserData.UserId, IncomingConnection.UserId, "decline");
            string serializedP2PConnection = JsonSerializer.Serialize(p2PConnectionData);
            string message = $"P2PDeclined;{serializedP2PConnection}";
            webSocket.Send(message);
            listener.Listen();
            

        }

        private void ButtonAcceptConnection_Click(object sender, RoutedEventArgs e)
        {
            P2PConnectionData p2PConnectionData = new P2PConnectionData(UserData.UserId, IncomingConnection.UserId, "accept");
            string serializedP2PConnection = JsonSerializer.Serialize(p2PConnectionData);
            string message = $"P2PAccepted;{serializedP2PConnection}";
            webSocket.Send(message);
        }
    }
}
