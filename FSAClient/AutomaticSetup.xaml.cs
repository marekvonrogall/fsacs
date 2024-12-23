using FSAClient.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for AutomaticSetup.xaml
    /// </summary>
    public partial class AutomaticSetup : Page
    {
        private Setup setup = new Setup();

        public AutomaticSetup()
        {
            InitializeComponent();
            InitializeUI();
        }

        private async void InitializeUI()
        {
            MainWindow.Instance.NavigateToPage(this);
            await StartFSAClient();
        }

        private async Task StartFSAClient()
        {
            ButtonRetry.IsEnabled = false;
            ButtonManualSetup.IsEnabled = false;
            LabelStatusMessage.Content = "Suche nach Servern in deinen Netzwerken...";
            setup.labelStatusMessage = LabelStatusMessage;
            await setup.PopulateNetworkInterfaces();
            await setup.CheckValidNetworkInterfaces(ButtonRetry, ButtonManualSetup);
        }

        private async void ButtonRetry_Click(object sender, RoutedEventArgs e)
        {
            await StartFSAClient();
        }
    }
}
