﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for Username.xaml
    /// </summary>
    public partial class Username : Page
    {
        private Setup _setupPage;

        public Username(Setup setupPage)
        {
            InitializeComponent();
            _setupPage = setupPage;
        }

        private void ButtonRegisterClient_Click(object sender, RoutedEventArgs e)
        {
            _setupPage.FinishSetup(TextBoxUsername.Text);
        }
    }
}
