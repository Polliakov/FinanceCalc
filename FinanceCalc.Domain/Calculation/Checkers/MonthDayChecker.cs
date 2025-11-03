namespace FinanceCalc.Domain.Calculation.Checkers.Models
{
    public class MonthDayChecker(int monthDay, DateTime startDate) : ICanActivateChecker
    {
        protected readonly int _monthDay = monthDay;
        protected readonly DateTime _startDate = startDate;

        public bool CanActivate(DateTime currentTime)
        {
            return currentTime.ToString("dd") == _monthDay.ToString("d2");
        }
    }
}
