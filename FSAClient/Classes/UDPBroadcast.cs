using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FSAClient.Classes
{
    public class UDPBroadcast
    {
        public async Task<string> LookForServerAsync(IPAddress localAddress)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient(new IPEndPoint(localAddress, 0)))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 17551);
                    UdpClient listener = new UdpClient(new IPEndPoint(localAddress, 17550));
                    listener.EnableBroadcast = true;

                    string message = "DISCOVER_WEBSOCKET_SERVER";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    await udpClient.SendAsync(data, data.Length, endPoint);

                    var receiveTask = listener.ReceiveAsync();
                    var timeoutTask = Task.Delay(1000);

                    // Wait for either the response or the timeout
                    var completedTask = await Task.WhenAny(receiveTask, timeoutTask);
                    listener.Close();
                    listener.Dispose();

                    if (completedTask == timeoutTask)
                    {
                        return "NO_SERVER_FOUND";
                    }

                    var receivedData = await receiveTask;
                    string responseMessage = Encoding.UTF8.GetString(receivedData.Buffer);
                    return responseMessage;
                }
            }
            catch(Exception ex)
            {
                return "NO_SERVER_FOUND";
            }
        }
    }
}
