using System.Windows;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Controls;
using FSAClient.Classes;
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

            //CONNECTION ALERT TEST
            ConnectionAlert test = new ConnectionAlert(CreateSerializedString());
            test.Show();
        }

        //SAMPLE DATA FOR TESTING PURPOSES
        public string CreateSerializedString()
        {
            ConnectionAlertData data = new ConnectionAlertData
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

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
