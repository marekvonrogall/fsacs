using System.Windows;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text.Json;

namespace FSAClient
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            NavigateToPage(new Setup());
            
            ConnectionAlert test =new ConnectionAlert(CreateSerializedString());
            test.Show();
        }

        public string CreateSerializedString()
        {
            ConnectionAlertData1 data = new ConnectionAlertData1
            {
                UserName = "Manuel",
                UserId = 1,
                IpAddress = "172.0.0.1",
                Port = 8080,
                FileName = "Brainrot",
                FileSize = "5TB"
            };
           return  JsonSerializer.Serialize(data);
        }
        public class ConnectionAlertData1
        {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public string IpAddress { get; set; }
            public int Port { get; set; }
            public string FileName { get; set; }
            public string FileSize { get; set; }
        }
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
