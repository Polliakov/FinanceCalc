namespace FinanceCalc.Utils
{
    public static class Compare
    {
        public static bool Like(string? text, string? value)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return value.Contains(text, StringComparison.OrdinalIgnoreCase);
        }

        public static bool InRangeDecimal(string minText, string maxText, decimal value)
        {
            if (!decimal.TryParse(
                minText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var min))
                min = decimal.MinValue;
            if (!decimal.TryParse(
                maxText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var max))
                max = decimal.MaxValue;
            return InRangeDecimal(min, max, value);
        }

        public static bool InRangeDecimal(decimal min, decimal max, decimal value)
        {
            return value >= min && value <= max;
        }

        public static bool InRangeNullableDecimal(string minText, string maxText, decimal? value)
        {
            if (value is null)
                return string.IsNullOrWhiteSpace(minText) && string.IsNullOrWhiteSpace(maxText);
            return InRangeDecimal(minText, maxText, value.Value);
        }

        public static bool InRangeNullableDecimal(decimal? min, decimal? max, decimal? value)
        {
            if (value is null)
                return !min.HasValue && !max.HasValue;
            return InRangeDecimal(min ?? decimal.MinValue, max ?? decimal.MaxValue, value.Value);
        }

        public static bool InRangeNullableInt(string minText, string maxText, int? value)
        {
            if (value is null)
                return string.IsNullOrWhiteSpace(minText) && string.IsNullOrWhiteSpace(maxText);
            if (!int.TryParse(
                minText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var min))
                min = int.MinValue;
            if (!int.TryParse(
                maxText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var max))
                max = int.MaxValue;

            return value.Value >= min && value.Value <= max;
        }

        public static bool InRangeDouble(string minText, string maxText, double value)
        {
            if (!double.TryParse(
                minText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var min)) min = double.MinValue;
            if (!double.TryParse(
                maxText,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var max)) max = double.MaxValue;

            return value >= min && value <= max;
        }

        public static bool InRangeNullableDouble(double? min, double? max, double value)
        {
            return (!min.HasValue || value >= min.Value) && (!max.HasValue || value <= max.Value);
        }

        public static double ParseDoubleOr(string text, double defaultValue)
            => double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : defaultValue;
    }
}
