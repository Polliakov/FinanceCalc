using FinanceCalc.Application.Catalog.Services;
using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models;
using System.ComponentModel; // Added for ICollectionView
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static FinanceCalc.Utils.Compare;
using System.Globalization;
using FinanceCalc.Domain.Models.Bonds;

namespace FinanceCalc.Forms
{
    public partial class BondsCatalogWindow : Window
    {
        private readonly IBondsService _service;
        private List<IBond> _allItems = [];
        private ICollectionView? _view;
        private BondMetrics? _lastMetrics;
        private readonly CancellationTokenSource _cts = new();

        public BondsCatalogWindow(IBondsService service)
        {
            InitializeComponent();
            _service = service;
            Loaded += BondsCatalogWindow_Loaded;
            Closing += (s, e) => _cts.Cancel();
        }

        private async void BondsCatalogWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            try
            {
                _allItems = [.. await _service.GetAllAsync()];

                var context = await _service.GetMetadataAsync(_cts.Token);
                new SimpleRelevanceResolver().CalculateRelevance(_allItems, context);

                UpdateCounts();
                _view = CollectionViewSource.GetDefaultView(_allItems);
                _view.Filter = FilterPredicate;
                BondsGrid.ItemsSource = _view;
                UpdateFoundCount();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error loading bonds: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMetricsAsync()
        {
            try
            {
                _lastMetrics = await _service.GetLastMetricsAsync(_cts.Token);
                if (FindName("MetricsView") is Controls.BondMetricsView metricsView)
                {
                    metricsView.Metrics = _lastMetrics;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error loading metrics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FilterText(string name) => (FindName(name) as TextBox)?.Text ?? string.Empty;

        private bool FilterPredicate(object obj)
        {
            if (obj is not IBond bond)
                return false;
            var noOfferFilter = (FindName("NoOfferFilter") as CheckBox)?.IsChecked;
            var needQualificationFilter = (FindName("NeedQualificationFilter") as CheckBox)?.IsChecked;

            var durationMin = ParseDoubleFieldOrNull("DurationFilterMinBox") ?? 0;
            var durationMax = ParseDoubleFieldOrNull("DurationFilterMaxBox");

            var profPerDurationMin = ParseDoubleFieldOrNull("ProfitabilityPerDurationMinBox");
            var profPerDurationMax = ParseDoubleFieldOrNull("ProfitabilityPerDurationMaxBox");

            var profMin = ParseDoubleFieldOrNull("ProfitabilityYearFilterMinBox");
            var profMax = ParseDoubleFieldOrNull("ProfitabilityYearFilterMaxBox");

            if (durationMax.HasValue)
            {
                var durationKoef = (bond.DurationYears - durationMin) / (durationMax.Value - durationMin);
                durationKoef = Math.Clamp(durationKoef, 0, 1);
                if (profMin.HasValue && profPerDurationMin.HasValue)
                    profMin += (profPerDurationMin - profMin) * durationKoef;
                if (profMax.HasValue && profPerDurationMax.HasValue)
                    profMax += (profPerDurationMax - profMax) * durationKoef;
            }

            bool passes =
                Like(FilterText("NameFilterBox"), bond.Name) &&
                Like(FilterText("TickerFilterBox"), bond.Ticker) &&
                InRangeDecimal(FilterText("NominalFilterMinBox"), FilterText("NominalFilterMaxBox"), bond.Nominal) &&
                InRangeDecimal(FilterText("CostFilterMinBox"), FilterText("CostFilterMaxBox"), bond.Cost / (bond.Nominal == 0 ? 1 : bond.Nominal) * 100) &&
                InRangeDecimal(FilterText("CapitalProfYearFilterMinBox"), FilterText("CapitalProfYearFilterMaxBox"), bond.CapitalProfitabilityYear * 100) &&
                InRangeNullableDecimal(FilterText("CouponFilterMinBox"), FilterText("CouponFilterMaxBox"), bond.Coupon) &&
                InRangeNullableInt(FilterText("CouponsPerYearFilterMinBox"), FilterText("CouponsPerYearFilterMaxBox"), bond.CouponsPerYear) &&
                InRangeNullableDecimal(FilterText("CouponProfYearFilterMinBox"), FilterText("CouponProfYearFilterMaxBox"), bond.CouponProfitabilityYear * 100) &&
                InRangeNullableDouble(durationMin, durationMax, bond.DurationYears) &&
                InRangeNullableDouble(profMin, profMax, (double)bond.ProfitabilityYear * 100) &&
                (noOfferFilter != true || !bond.OfferDate.HasValue) &&
                (needQualificationFilter != true || bond.NeedQualification);

            return passes;

            double? ParseDoubleFieldOrNull(string fieldName)
            {
                return double.TryParse(FilterText(fieldName), NumberStyles.Currency, CultureInfo.InvariantCulture, out var value) ?
                    value : null;
            }
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            _view?.Refresh();
            UpdateFoundCount();
        }

        private void UpdateCounts()
        {
            if (FindName("TotalCountText") is TextBlock total)
            {
                total.Text = _allItems.Count.ToString();
            }
        }

        private void UpdateFoundCount()
        {
            if (FindName("FoundCountText") is TextBlock found && _view is not null)
            {
                found.Text = _view.Cast<object>().Count().ToString();
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var editor = new BondEditorWindow(null);
            editor.Owner = this;
            if (editor.ShowDialog() == true)
            {
                await _service.AddAsync(editor.Result);
            }
            await ReloadAsync();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (BondsGrid.SelectedItem is not IBond bond)
                return;

            var editor = new BondEditorWindow(bond);
            editor.Owner = this;
            if (editor.ShowDialog() == true)
            {
                await _service.UpdateAsync(editor.Result);
            }
            await ReloadAsync();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (BondsGrid.SelectedItem is not IBond bond)
                return;

            var viewer = new BondViewerWindow(bond);
            viewer.Owner = this;
            viewer.ShowDialog();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BondsGrid.SelectedItem is not IBond bond)
                return;
            var res = MessageBox.Show(this,
                $"Delete bond '{bond.Name}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
                return;
            await _service.DeleteAsync(bond.Ticker);
            await ReloadAsync();
        }

        private async void FetchMoex_Click(object sender, RoutedEventArgs e)
        {
            await RunWithLoading(async (updateStatus) =>
            {
                updateStatus("Importing data from MOEX...");
                await _service.ImportFromMoex();
                updateStatus("Calculating metrics...");
                await CalculateAndSaveMetrics(updateStatus);
                updateStatus("Reloading grid...");
                await ReloadAsync();
            }, "Fetching Bonds Data...");
        }

        private async void CalculateMetrics_Click(object sender, RoutedEventArgs e)
        {
            await RunWithLoading(CalculateAndSaveMetrics);
        }

        private async Task RunWithLoading(Func<Action<string>, Task> action, string loadingTitle = "Loading...")
        {
            LoadingOverlay.Visibility = Visibility.Visible;
            LoadingTextBlock.Text = loadingTitle;
            try
            {
                await action(
                    status => LoadingStatusTextBlock.Text = status);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                LoadingTextBlock.Text = "Loading...";
            }
        }

        private async Task CalculateAndSaveMetrics(Action<string> updateStatus)
        {
            try
            {
                var metrics = await _service.CalculateMetricsAsync(10, _cts.Token);
                await _service.SaveMetricsAsync(metrics, _cts.Token);
                if (FindName("MetricsView") is Controls.BondMetricsView metricsView)
                {
                    metricsView.Metrics = metrics;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error calculating metrics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BondsGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not TextBlock textBlock) return;

            var cell = FindParent<DataGridCell>(textBlock);
            if (cell == null) return;

            var text = textBlock.Text;
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                Clipboard.SetText(text);
                if (FindName("CopiedValueText") is TextBlock copiedValueText && FindName("ClipboardPopup") is System.Windows.Controls.Primitives.Popup clipboardPopup)
                {
                    copiedValueText.Text = $"Copied: {text}";
                    clipboardPopup.IsOpen = true;

                    var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                    timer.Tick += (s, args) =>
                    {
                        clipboardPopup.IsOpen = false;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to copy to clipboard: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            return parentObject is T parent ? parent : FindParent<T>(parentObject);
        }
    }
}
