using FinanceCalc.Domain.Abstractions;
using FinanceCalc.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace FinanceCalc.Forms
{
    public partial class BondEditorWindow : Window
    {
        public BondEditorWindow(IReadOnlyBondData? bondData = null)
        {
            InitializeComponent();

            ViewModel = new BondEditorViewModel(bondData);
            DataContext = ViewModel;
        }

        public BondEditorViewModel ViewModel { get; }
        public IBond Result => ViewModel.Computed;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Recalculate();
            DialogResult = true;
            Close();
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Recalculate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Ignore calculation errors for now.
            }
        }
    }
}
