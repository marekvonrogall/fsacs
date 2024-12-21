using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace FSAServerCLI.Classes
{
    public class NetworkInterfaces
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IPAddress Address { get; set; }
        public IPAddress BroadcastAddress { get; set; }
        public List<NetworkInterfaces> AllNetworkInterfaces { get; private set; }

        public void ListNetworkInterfaces()
        {
            AllNetworkInterfaces = new List<NetworkInterfaces>();
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                if (networkInterface.OperationalStatus != OperationalStatus.Up) continue;

                var ipProperties = networkInterface.GetIPProperties();

                foreach (var ipAddress in ipProperties.UnicastAddresses)
                {
                    if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork) //IPv4
                    {
                        var broadcastAddress = CalculateBroadcastAddress(ipAddress.Address, ipAddress.IPv4Mask);

                        var ni = new NetworkInterfaces
                        {
                            Name = networkInterface.Name,
                            Description = networkInterface.Description,
                            Address = ipAddress.Address,
                            BroadcastAddress = broadcastAddress
                        };
                        AllNetworkInterfaces.Add(ni);
                        Console.WriteLine($"[{AllNetworkInterfaces.Count}]: {ni.Name} - {ni.Description}");
                    }
                }
            }
        }

        private IPAddress CalculateBroadcastAddress(IPAddress localAddress, IPAddress subnetMask)
        {
            byte[] localIpBytes = localAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (localIpBytes.Length != subnetMaskBytes.Length)
                throw new InvalidOperationException("Local IP address and subnet mask have different byte lengths.");

            byte[] broadcastAddressBytes = new byte[localIpBytes.Length];

            // Perform bitwise OR between local IP and inverted subnet mask to get the broadcast address
            for (int i = 0; i < localIpBytes.Length; i++)
            {
                broadcastAddressBytes[i] = (byte)(localIpBytes[i] | (byte)~subnetMaskBytes[i]);
            }

            return new IPAddress(broadcastAddressBytes);
        }
    }
}
