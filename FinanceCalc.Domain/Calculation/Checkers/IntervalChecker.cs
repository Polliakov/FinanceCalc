namespace FinanceCalc.Domain.Calculation.Checkers.Models
{
    public class IntervalChecker(int daysPeriod, bool activateOnZeroDay, DateTime startDate) : ICanActivateChecker
    {
        protected readonly int _daysPeriod = daysPeriod;
        protected readonly bool _activateOnZeroDay = activateOnZeroDay;
        protected readonly DateTime _startDate = startDate;

        public bool CanActivate(DateTime currentTime)
        {
            var daysLeft = (currentTime - _startDate).TotalDays;
            if (daysLeft == 0 && _activateOnZeroDay)
                return true;

            return daysLeft % _daysPeriod == 0;
        }
    }
}
