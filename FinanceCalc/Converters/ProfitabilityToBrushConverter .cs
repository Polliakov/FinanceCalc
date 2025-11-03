using FinanceCalc.Domain.Models.Primitives;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FinanceCalc.Converters
{
    public class ProfitabilityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (str.TrimStart() is { Length: > 0 } s)
                {
                    return s[0] == '-' ? Brushes.Red : Brushes.Green;
                }
            }
            if (value is ComplexPercent percent)
            {
                return ConvertInternal((double)percent.Value);
            }
            if (TryConvertToDouble(value, out var number))
            {
                return ConvertInternal(number);
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static object ConvertInternal(double number)
        {
            return number switch
            {
                > 0 => Brushes.Green,
                < 0 => Brushes.Red,
                _ => Brushes.Black,
            };
        }

        private bool TryConvertToDouble(object value, out double result)
        {
            try
            {
                result = System.Convert.ToDouble(value);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }
    }
}
