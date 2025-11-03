using FinanceCalc.Domain.Abstractions;
using System.Windows;

namespace FinanceCalc.Forms
{
    public partial class BondViewerWindow : Window
    {
        public BondViewerWindow(IBond bond)
        {
            DataContext = bond;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
