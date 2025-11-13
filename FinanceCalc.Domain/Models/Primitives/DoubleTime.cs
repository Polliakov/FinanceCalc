namespace FinanceCalc.Domain.Models.Primitives
{
    public class DoubleTime
    {
        public const int MonthsInYear = 12;

        public const int DaysInYear = 365;
        public const double DaysInMonth = (double)DaysInYear / MonthsInYear;

        public const int HoursInDay = 24;
        public const double HoursInMonth = DaysInMonth * HoursInDay;
        public const int HoursInYear = DaysInYear * HoursInDay;

        public const int Year = 1;
        public const double Month = (double)Year / MonthsInYear;
        public const double Day = Month / DaysInMonth;
        public const double Hour = Day / HoursInDay;

        public static double Months(double count) => count / MonthsInYear;
        public static double Days(double count) => count / DaysInYear;
        public static double Hours(double count) => count / HoursInDay;

        public static double Time(double years, double months = 0, double days = 0, double hours = 0)
        {
            return years
                + Months(months)
                + Days(days)
                + Hours(hours);
        }

        public static double FromTimeSpan(TimeSpan timeSpan)
        {
            return timeSpan.TotalHours / HoursInYear;
        }

        public static TimeSpan ToTimeSpan(double time)
        {
            return TimeSpan.FromHours(time * HoursInYear);
        }
    }
}
