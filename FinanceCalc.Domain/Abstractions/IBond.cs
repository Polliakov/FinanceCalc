namespace FinanceCalc.Domain.Abstractions
{
    public interface IBond : IReadOnlyBondData
    {
        double DurationYears { get; }
        double? CouponsPeriodMonths { get; }
        decimal? CouponProfitability { get; }
        decimal? CouponProfitabilityYear { get; }
        decimal CapitalProfitability { get; }
        decimal CapitalProfitabilityYear { get; }
        decimal ProfitabilityYear { get; }
    }
}