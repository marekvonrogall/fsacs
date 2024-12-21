using System;
using System.Net;

namespace FSAClient.Classes
{
    public static class UserData
    {
        public static string Name { get; private set; }
        public static int UserId { get; set; }

        public static IPAddress LocalIP { get; private set; }
        public static int LocalPort { get; private set; }

        public static bool StoreUserData(string name, string localIP, string localPort)
        {
            try //VALIDATE INPUTS HERE!!! (NOT DONE YET)
            {
                Name = name;
                LocalIP = IPAddress.Parse(localIP);
                LocalPort = Convert.ToInt32(localPort);

                return true;
            }
            catch { return false; }
        }
    }
}
