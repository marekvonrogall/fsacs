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

            
        }

       
       
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
