using System.Net;

namespace FSAClient.Classes
{
    public static class UserData
    {
        public static string Name { get; private set; }
        public static int UserId { get; set; }

        public static IPAddress LocalIP { get; private set; }
        public static int LocalPort { get; private set; }

        public static bool IsValid { get; private set; }

        public static void StoreUserData(string name, IPAddress localIp, int localPort)
        {
            try //VALIDATE INPUTS HERE!!! (NOT DONE YET)
            {
                Name = name;
                LocalIP = localIp;
                LocalPort = localPort;

                IsValid = true;
            }
            catch { IsValid = false; }
        }
    }
}
