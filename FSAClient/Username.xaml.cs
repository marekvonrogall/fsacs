using System.Windows;
using System.Windows.Controls;
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
