using System;
using System.Net;

namespace FSAClient.Classes
{
    public static class UserData
    {
        public static string Name { get; private set; }
        public static int UserId { get; set; } //Retreived from Server

        public static IPAddress LocalIP { get; private set; }
        public static int LocalPort { get; private set; }

        public static IPAddress PublicIP { get; private set; }
        public static int PublicPort { get; private set; }

        public static bool StoreUserData(string name, string localIP, string localPort, string publicIP, string publicPort)
        {
            try //VALIDATE INPUTS HERE!!! (NOT DONE YET)
            {
                Name = name;
                LocalIP = IPAddress.Parse(localIP);
                LocalPort = Convert.ToInt32(localPort);
                PublicIP = IPAddress.Parse(publicIP);
                PublicPort = Convert.ToInt32(publicPort);

                return true;
            }
            catch { return false; }
        }
    }
}
