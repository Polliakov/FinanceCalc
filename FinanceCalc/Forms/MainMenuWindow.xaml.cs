using FinanceCalc.Forms.Factory;
using System.Windows;

namespace FinanceCalc.Forms
{
    public partial class MainMenuWindow : Window
    {
        private readonly IFormsFactory _formsFactory;

        public MainMenuWindow(IFormsFactory formsFactory)
        {
            InitializeComponent();
            _formsFactory = formsFactory;
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = _formsFactory.CreateDashboardWindow();
            wnd.Owner = this;
            wnd.Show();
        }

        private void BondsCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = _formsFactory.CreateBondsCatalogWindow();
            wnd.Owner = this;
            wnd.Show();
        }
    }
}
