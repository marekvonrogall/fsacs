using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace FSAClient.Classes
{
    public class IPServices
    {
        public IPAddress GetLocalIP() //https://stackoverflow.com/questions/6803073/get-local-ip-address
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public async Task<IPAddress> GetPublicIP() //Get Public IP-Address. Code from https://www.ipify.org/
        {
            try
            {
                var httpClient = new HttpClient();
                var ip = await httpClient.GetStringAsync("https://api.ipify.org");
                return IPAddress.Parse(ip);
            }
            catch { return null; }
        }
    }
}
