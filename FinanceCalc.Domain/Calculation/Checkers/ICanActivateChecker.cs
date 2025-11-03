namespace FinanceCalc.Domain.Calculation.Checkers.Models
{
    public interface ICanActivateChecker
    {
        bool CanActivate(DateTime currentTime);
    }
}
