using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Net;

namespace FSAClient.Classes
{
    public class Listener
    {
        
        public async void Listen()
        {
            IPAddress localIP = UserData.LocalIP;
            int localPort = UserData.LocalPort;

            TcpListener listener = new TcpListener(localIP, localPort);
            listener.Start();

            MessageBox.Show($"Listening on {localIP}:{localPort}");

            await Task.Run(() =>
            {
                TcpClient client = listener.AcceptTcpClient();
                MessageBox.Show("Remote client successfully connected!");
            });
        }
    }
}
