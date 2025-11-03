namespace FinanceCalc.Domain.Calculation.View
{
    public class Report(
        DateTime date, 
        decimal actualInflation,
        decimal worthSum, 
        decimal incomeWorthSum,
        decimal addingPercent,
        decimal addingIncomePercent)
    {
        public DateTime Date { get; } = date;
        public decimal ActualInflation { get; } = actualInflation;
        public decimal WorthSum { get; } = worthSum;
        public decimal WorthAdding => WorthSum * AddingPercent;
        public decimal Worth => WorthSum * (1 - AddingPercent);
        public decimal IncomeWorthSum { get; } = incomeWorthSum;
        public decimal IncomeWorthAdding => IncomeWorthSum * AddingIncomePercent;
        public decimal IncomeWorth => IncomeWorthSum * (1 - AddingIncomePercent);
        public decimal AddingPercent { get; } = addingPercent;
        public decimal AddingIncomePercent { get; } = addingIncomePercent;

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} > actual inflation: {ActualInflation * 100 ,7:f3}% worth: {WorthSum,12:f2}";
        }
    }
}
