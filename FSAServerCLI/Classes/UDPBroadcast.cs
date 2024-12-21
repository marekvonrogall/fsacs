using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace FSAServerCLI.Classes
{
    public class UDPBroadcast
    {
        public static async void StartUdpBroadcastServer(NetworkInterfaces selectedNetworkInterface, string responseMessage)
        {
            try
            {
                Console.WriteLine("UDP Server: Listening for UDP discovery requests...");
                Console.WriteLine($"UDP Server: {selectedNetworkInterface.Address}:17551");

                IPEndPoint endPoint = new IPEndPoint(selectedNetworkInterface.BroadcastAddress, 17550);
                UdpClient listener = new UdpClient(new IPEndPoint(selectedNetworkInterface.Address, 17551));

                while (true)
                {
                    using (UdpClient udpClient = new UdpClient(new IPEndPoint(selectedNetworkInterface.Address, 0)))
                    {
                        listener.EnableBroadcast = true;

                        var receivedData = await listener.ReceiveAsync();

                        if (Encoding.UTF8.GetString(receivedData.Buffer) == "DISCOVER_WEBSOCKET_SERVER")
                        {
                            byte[] data = Encoding.UTF8.GetBytes(responseMessage);
                            await udpClient.SendAsync(data, data.Length, endPoint);
                            Console.WriteLine("Received UDP Request, sent back WebSocket-Address!");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("This machine already hosts a server! You can host another server, but it will not react to UDP requests!");
                //Environment.Exit(0);
            }
        }
    }
}
