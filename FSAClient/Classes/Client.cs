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

                        fileNameLength.CopyTo(clientData, 0);
                        fileNameData.CopyTo(clientData, 4);
                        fileData.CopyTo(clientData, 4 + fileNameData.Length);

                        TcpClient client = new TcpClient();
                        client.Connect(publicIP, publicPort);
                        NetworkStream stream = client.GetStream();
                        stream.Write(clientData, 0, clientData.Length);
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
