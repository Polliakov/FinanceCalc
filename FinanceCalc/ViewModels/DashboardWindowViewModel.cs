using OxyPlot;
using System.ComponentModel;
using System.Globalization;

namespace FinanceCalc.ViewModels
{
    public class DashboardWindowViewModel : INotifyPropertyChanged
    {
        public PlotModel? WorthPlotModel { get; set; }
        public PlotModel? WorthIncomePlotModel { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Parse("2025-10-21");
        public DateTime EndDate { get; set; } = DateTime.Parse("2035-10-21");
        public decimal CurrentWorth { get; set; } = 100_000m;
        public decimal InflationYearRate { get; set; } = 0.075m;
        public decimal IncomeYearRate { get; set; } = 0.16m;
        public decimal WorthAddingInMonth { get; private set; } = 10_000m;
        public string WorthAddingInMonthText
        {
            get => WorthAddingInMonth.ToString();
            set
            {
                if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var val))
                WorthAddingInMonth = val;
                OnPropertyChanged(nameof(WorthAddingInMonthText));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
