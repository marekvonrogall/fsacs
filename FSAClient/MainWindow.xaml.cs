using System.Windows;
using System.Windows.Controls;

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
