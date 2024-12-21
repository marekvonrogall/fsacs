using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Controls;

namespace FSAClient.Classes
{
    public class NetworkInterfaces
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IPAddress Address { get; set; }

        public void PopulateNetworkInterfaces(ComboBox ComboBoxNetworkInterfaces)
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                var ipProperties = networkInterface.GetIPProperties();

                foreach (var ipAddress in ipProperties.UnicastAddresses)
                {
                    if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork) //IPv4
                    {
                        var ni = new NetworkInterfaces
                        {
                            Name = networkInterface.Name,
                            Description = networkInterface.Description,
                            Address = ipAddress.Address
                        };
                        var newItem = new ComboBoxItem
                        {
                            Content = ($"{ni.Name} - {ni.Description}"),
                            Tag = ni
                        };
                        ComboBoxNetworkInterfaces.Items.Add(newItem);
                    }
                }
            }
        }
    }
}
