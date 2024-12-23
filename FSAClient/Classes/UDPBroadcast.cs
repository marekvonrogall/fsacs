using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FSAClient.Classes
{
    public class UDPBroadcast
    {
        public async Task<string> LookForServerAsync(NetworkInterfaces selectedNetworkInterface)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient(new IPEndPoint(selectedNetworkInterface.Address, 0)))
                {
                    udpClient.EnableBroadcast = true;

                    IPEndPoint endPoint = new IPEndPoint(selectedNetworkInterface.BroadcastAddress, 17551);

                    string message = "DISCOVER_WEBSOCKET_SERVER";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    using (UdpClient listener = new UdpClient(new IPEndPoint(selectedNetworkInterface.Address, 17550)))
                    {
                        await udpClient.SendAsync(data, data.Length, endPoint);
                        listener.EnableBroadcast = true;

                        var receiveTask = listener.ReceiveAsync();
                        var timeoutTask = Task.Delay(2000);

                        var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                        if (completedTask == timeoutTask)
                        {
                            return "NO_SERVER_FOUND";
                        }

                        var receivedData = await receiveTask;
                        string responseMessage = Encoding.UTF8.GetString(receivedData.Buffer);
                        return responseMessage;
                    }
                }
            }
            catch
            {
                return "NO_SERVER_FOUND";
            }
        }

    }
}
