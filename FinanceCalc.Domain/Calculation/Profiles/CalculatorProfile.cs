namespace FinanceCalc.Domain.Calculation.Profiles
{
    public class CalculatorProfile
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public Dictionary<string, WorthAdditive> WorthAdditives { get; set; } = [];
    }
}
