using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.IO;
using Microsoft.Win32;

namespace FSAClient.Classes
{
    public class Listener
    {
        private TcpListener listener;

        public async void Listen()
        {
            listener = new TcpListener(UserData.LocalIP, UserData.LocalPort);
            await Task.Run(ListenForNetworkStream);
        }

        private void ListenForNetworkStream()
        {
            try
            {
                listener.Start();
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

                SaveFile(fileName, fileData);
                listener.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveFile(string fileName, byte[] fileData)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save File As",
                Filter = "All Files|*.*",
                DefaultExt = Path.GetExtension(fileName),
                FileName = fileName
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllBytes(filePath, fileData);
                    MessageBox.Show($"File saved successfully to {filePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Save operation canceled.");
            }
        }
    }
}
