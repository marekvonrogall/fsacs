﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        private IPServices iPService = new IPServices();

        public Setup()
        {
            InitializeComponent();
            GetClientInfo();
        }

        private async void GetClientInfo()
        {
            try
            {
                IPAddress localIP = iPService.GetLocalIP();
                IPAddress publicIP = await iPService.GetPublicIP();

                TextBoxLocalIP.Text = localIP.ToString();
                TextBoxPublicIP.Text = publicIP.ToString();
            }
            catch { }
        }

        private void ButtonFinishSetup_Click(object sender, RoutedEventArgs e)
        {
            if(UserData.StoreUserData(TextBoxName.Text, TextBoxLocalIP.Text, TextBoxLocalPort.Text, TextBoxPublicIP.Text, TextBoxPublicPort.Text))
            {
                var newPage = new FSA();
                MainWindow.Instance.NavigateToPage(newPage);
            }
            else { MessageBox.Show("Ungültige Eingaben erkannt.", "Error"); }
        }
    }
}
