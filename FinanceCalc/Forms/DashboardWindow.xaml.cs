using FinanceCalc.Domain.Calculation;
using FinanceCalc.Domain.Calculation.Profiles;
using FinanceCalc.Domain.Calculation.View;
using FinanceCalc.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;

namespace FinanceCalc.Forms
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            DataContext = _model;

            //var viewer = new BondViewerWindow(new Bond());
            //viewer.Show();
        }

        private readonly Calculator _calculator = new();
        public readonly DashboardWindowViewModel _model = new();

        private void Recalculate()
        {
            var reports = Calculator.Main(new CalculatorProfile
            {
                StartDate = _model.StartDate,
                EndDate = _model.EndDate,
                WorthAdditives = { { "Adding", new WorthAdditive { Name = "Adding", InMonth = _model.WorthAddingInMonth } } },
            });
            CreatePlot(reports);
        }

        private void CreatePlot(IEnumerable<Report> reports)
        {
            var worthModel = new PlotModel { Title = "Worth" };

            worthModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time", MinorStep = 1, MajorStep = 12 });
            worthModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Value", UseSuperExponentialFormat = false, MinorStep = 100_000 });

            var worthIncomeModel = new PlotModel { Title = "Worth Income" };

            worthIncomeModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time", MinorStep = 1, MajorStep = 12 });
            worthIncomeModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Value" });

            var worthSumLine = CreateLine("Worth Sum");
            var worthLine = CreateLine("Worth");
            var worthAddingLine = CreateLine("Worth Adding");
            var worthIncomeSumLine = CreateLine("Income Worth Sum");
            var worthIncomeLine = CreateLine("Income Worth");
            var worthIncomeAddingLine = CreateLine("Income Worth Adding");

            var worthOptimumColor = OxyColor.FromRgb(30, 30, 210);
            var worthOptimumMarker = CreateLine("Worth Optimum", worthOptimumColor);
            var worthIncomeOptimumMarker = CreateLine("Worth Income Optimum", worthOptimumColor);


            foreach (var (report, index) in reports.Select((r, i) => (r, i + 1)))
            {
                worthSumLine.Points.Add(new(index, (double)Math.Round(report.WorthSum, 2)));
                if (report.Worth > report.WorthAdding)
                    worthOptimumMarker.Points.Add(new(index, (double)Math.Round(report.Worth, 2)));
                else
                    worthLine.Points.Add(new(index, (double)Math.Round(report.Worth, 2)));
                worthAddingLine.Points.Add(new(index, (double)Math.Round(report.WorthAdding, 2)));

                worthIncomeSumLine.Points.Add(new(index, (double)Math.Round(report.IncomeWorthSum, 2)));
                if (report.IncomeWorth > report.IncomeWorthAdding)
                    worthIncomeOptimumMarker.Points.Add(new(index, (double)Math.Round(report.IncomeWorth, 2)));
                else
                    worthIncomeLine.Points.Add(new(index, (double)Math.Round(report.IncomeWorth, 2)));
                worthIncomeAddingLine.Points.Add(new(index, (double)Math.Round(report.IncomeWorthAdding, 2)));
            }

            worthModel.Series.Add(worthSumLine);
            worthModel.Series.Add(worthLine);
            worthModel.Series.Add(worthAddingLine);
            worthModel.Series.Add(worthOptimumMarker);

            worthIncomeModel.Series.Add(worthIncomeSumLine);
            worthIncomeModel.Series.Add(worthIncomeLine);
            worthIncomeModel.Series.Add(worthIncomeAddingLine);
            worthIncomeModel.Series.Add(worthIncomeOptimumMarker);


            _model.WorthPlotModel = worthModel;
            _model.WorthIncomePlotModel = worthIncomeModel;
            WorthPlotView.Model = worthModel;
            WorthIncomePlotView.Model = worthIncomeModel;
        }

        private static LineSeries CreateLine(string title, OxyColor? color = null, int markerSize = 2)
        {
            return new()
            {
                Title = title,
                MarkerType = MarkerType.Circle,
                MarkerSize = markerSize,
                MarkerStroke = color ?? OxyColors.Automatic
            };
        }

        private void RecalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Recalculate();
        }
    }
}
