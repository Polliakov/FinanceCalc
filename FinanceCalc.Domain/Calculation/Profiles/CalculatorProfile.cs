namespace FinanceCalc.Domain.Calculation.Profiles
{
    public class CalculatorProfile
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public decimal CurrentWorth { get; set; } = 0m;
        public decimal InflationYearRate { get; set; } = 0.075m;
        public decimal IncomeYearRate { get; set; } = 0.16m;
        public Dictionary<string, WorthAdditive> WorthAdditives { get; set; } = [];
    }
}
