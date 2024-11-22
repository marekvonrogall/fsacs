using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace FSAClient.Classes
{
    public class Client
    {
        public async void EstablishConnection(int publicPort, IPAddress publicIP) //use remote IP & Port!
        {
            await Task.Run(() =>
            {
                TcpClient client = new TcpClient();
                client.Connect(publicIP, publicPort);
                MessageBox.Show("Successfully connected to remote client!");
            });
        }
    }
}
