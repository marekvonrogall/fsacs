using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Collections.Generic;
using System;
using System.IO;

namespace FSAClient.Classes
{
    public class Listener
    {
        
        public async void Listen()
        {
            IPAddress localIP = UserData.LocalIP;
            int localPort = UserData.LocalPort;

            TcpListener listener = new TcpListener(localIP, localPort);
            listener.Start();

            MessageBox.Show($"Listening on {localIP}:{localPort}");

            await Task.Run(() =>
            {
                try
                {

                
                TcpClient client = listener.AcceptTcpClient();

                
                NetworkStream stream = client.GetStream();
                    byte[] sizeBuffer = new byte[4];

                    stream.Read(sizeBuffer, 0, 4);
                    int fullDataSize = BitConverter.ToInt32(sizeBuffer, 0);
                    byte[] fullData = new byte[fullDataSize];
                    int bytesRead = stream.Read(fullData, 0, fullData.Length);

                    int fileNameLength = BitConverter.ToInt32(fullData, 0);
                string fileName = System.Text.Encoding.ASCII.GetString(fullData, 4, fileNameLength);
                byte[] fileData = new byte[fullData.Length - 4 - fileNameLength];

                Array.Copy(fullData, 4 + fileNameLength, fileData, 0, fileData.Length);

                File.WriteAllBytes(fileName, fileData); //Speicherort muss noch geändert werden.

                MessageBox.Show("Remote client received Data!");

                    listener.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}
