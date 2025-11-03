using FinanceCalc.Domain.Models;
using FinanceCalc.Domain.Models.Primitives;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;
using System.Windows.Controls;

namespace FinanceCalc.Controls
{
    public partial class BondMetricsView : UserControl
    {
        public BondMetricsView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty MetricsProperty = DependencyProperty.Register(
            nameof(Metrics), typeof(BondMetrics), typeof(BondMetricsView), new PropertyMetadata(null, OnMetricsChanged));

        public BondMetrics? Metrics
        {
            get => (BondMetrics?)GetValue(MetricsProperty);
            set => SetValue(MetricsProperty, value);
        }

        public PlotModel? CostDiffPlotModel
        {
            get => (PlotModel?)GetValue(CostDiffPlotModelProperty);
            set => SetValue(CostDiffPlotModelProperty, value);
        }

        public static readonly DependencyProperty CostDiffPlotModelProperty = DependencyProperty.Register(
            nameof(CostDiffPlotModel), typeof(PlotModel), typeof(BondMetricsView), new PropertyMetadata(null));

        public PlotModel? CouponYieldPlotModel
        {
            get => (PlotModel?)GetValue(CouponYieldPlotModelProperty);
            set => SetValue(CouponYieldPlotModelProperty, value);
        }

        public static readonly DependencyProperty CouponYieldPlotModelProperty = DependencyProperty.Register(
            nameof(CouponYieldPlotModel), typeof(PlotModel), typeof(BondMetricsView), new PropertyMetadata(null));

        public PlotModel? TotalYieldPlotModel
        {
            get => (PlotModel?)GetValue(TotalYieldPlotModelProperty);
            set => SetValue(TotalYieldPlotModelProperty, value);
        }

        public static readonly DependencyProperty TotalYieldPlotModelProperty = DependencyProperty.Register(
            nameof(TotalYieldPlotModel), typeof(PlotModel), typeof(BondMetricsView), new PropertyMetadata(null));

        public PlotModel? DurationPlotModel
        {
            get => (PlotModel?)GetValue(DurationPlotModelProperty);
            set => SetValue(DurationPlotModelProperty, value);
        }

        public static readonly DependencyProperty DurationPlotModelProperty = DependencyProperty.Register(
            nameof(DurationPlotModel), typeof(PlotModel), typeof(BondMetricsView), new PropertyMetadata(null));

        private static void OnMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BondMetricsView view)
            {
                view.BuildPlots();
            }
        }

        private void BuildPlots()
        {
            if (Metrics is null)
            {
                CostDiffPlotModel = null;
                CouponYieldPlotModel = null;
                TotalYieldPlotModel = null;
                DurationPlotModel = null;
                return;
            }

            CostDiffPlotModel = CreateHistogramModel(Metrics.CostDiffDistribution, isPercent: true, shift: -1.0);
            CouponYieldPlotModel = CreateHistogramModel(Metrics.CouponYieldYearDistribution, isPercent: true);
            TotalYieldPlotModel = CreateHistogramModel(Metrics.TotalYieldYearDistribution, isPercent: true);
            DurationPlotModel = CreateHistogramModel(Metrics.DurationYearsDistribution, isPercent: false);
        }

        private static PlotModel CreateHistogramModel(
            DistributionPoint[] distribution,
            bool isPercent,
            double shift = 0)
        {
            var model = new PlotModel { TitlePadding = 0 };
            var categoryAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 50,
            };

            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Count", IsZoomEnabled = false });

            var sum = distribution.Sum(p => p.Count);
            var percent = isPercent ? 100 : 1;
            var series = new HistogramSeries() { };
            foreach (var point in distribution)
            {
                var normalizedCount = (double)point.Count / sum * 100;
                series.Items.Add(new HistogramItem(
                    Math.Round((point.LowerBound + shift) * percent, 8), 
                    Math.Round((point.UpperBound + shift) * percent, 8),
                    Math.Round(normalizedCount * Math.Abs((point.UpperBound - point.LowerBound) * percent), 8),
                    (int)normalizedCount));
            }
            model.Series.Add(series);
            return model;
        }
    }
}
