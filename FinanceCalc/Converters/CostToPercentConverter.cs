using System.Globalization;
using System.Windows.Data;

namespace FinanceCalc.Converters
{
    public class CostToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Domain.Abstractions.IReadOnlyBondData bond && bond.Nominal != 0)
            {
                return (bond.Cost / bond.Nominal * 100m).ToString("N2", culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
