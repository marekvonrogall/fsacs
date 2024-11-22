using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System;

namespace FSAClient.Classes
{
    public class Client
    {
        public async void SendData( int publicPort, IPAddress publicIP)
        {
            await Task.Run(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        byte[] fileData = File.ReadAllBytes(filePath);
                        byte[] fileNameData = System.Text.Encoding.ASCII.GetBytes(Path.GetFileName(filePath));
                        byte[] fileNameLength = BitConverter.GetBytes(fileNameData.Length);
                        byte[] clientData = new byte[4 + fileNameData.Length + fileData.Length];
                        byte[] fullClientData = new byte[4 + clientData.Length];
                        byte[] totalSize = BitConverter.GetBytes(clientData.Length);

                        fileNameLength.CopyTo(clientData, 0);
                        fileNameData.CopyTo(clientData, 4);
                        fileData.CopyTo(clientData, 4 + fileNameData.Length);
                        totalSize.CopyTo(fullClientData, 0);
                        clientData.CopyTo(fullClientData, 4);

                        TcpClient client = new TcpClient();
                        client.Connect(publicIP, publicPort);
                        NetworkStream stream = client.GetStream();
                        stream.Write(fullClientData, 0, fullClientData.Length);
                        MessageBox.Show("Data sent to remote client!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }
    }
}
