using System.Net;
using System.Net.Sockets;

namespace FSAClient.Classes
{
    public class PortServices
    {
        public int GetFreeTcpPort(IPAddress ipAddress)
        {
            try
            {
                TcpListener listener = new TcpListener(ipAddress, 0);
                listener.Start();
                int port = ((IPEndPoint)listener.LocalEndpoint).Port;
                listener.Stop();
                return port;
            }
            catch
            {
                return 0;
            }
        }
    }
}
