using FSAClient.Classes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            StartFSAClient();
        }

        private void InitializeUI()
        {
            MainWindow.Instance.NavigateToPage(this);
            ButtonRetry.IsEnabled = false;
            ButtonManualSetup.IsEnabled = false;
            LabelStatusMessage.Content = "Suche nach Servern in deinen Netzwerken...";
        }

        private async Task StartFSAClient()
        {
            setup.labelStatusMessage = LabelStatusMessage;
            setup.comboBoxNetworkInterfaces = ComboBoxNetworkInterfaces;

            await setup.PopulateNetworkInterfaces();
            await setup.CheckValidNetworkInterfaces(ButtonRetry, ButtonManualSetup, ButtonUseNetwork);
        }

        private void ButtonRetry_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxNetworkInterfaces.Items.Clear();
            StartFSAClient();
        }

        private void ButtonManualSetup_Click(object sender, RoutedEventArgs e)
        {
            //ADD PAGE ManualSetup
            //MainWindow.Instance.NavigateToPage(new ManualSetup());
        }

        private void ButtonUseNetwork_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NavigateToPage(new Username(setup));
        }
    }
}
