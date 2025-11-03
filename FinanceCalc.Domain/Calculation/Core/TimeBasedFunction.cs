using FinanceCalc.Domain.Calculation.Checkers.Models;

namespace FinanceCalc.Domain.Calculation.Core
{
    public abstract class TimeBasedFunction(ICanActivateChecker checker)
    {
        public virtual string? Name { get; set; }
        protected ICanActivateChecker _checker = checker;

        public decimal Activate(DateTime currentTime, decimal sum)
        {
            if (_checker.CanActivate(currentTime))
                return ActivationAction(sum);

            return sum;
        }

        protected abstract decimal ActivationAction(decimal sum);
    }
}
