using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Domain.Entities
{
    public class BondMetricsEntity
    {
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

        public double MeanCostDiffPercent { get; set; }
        public double StdErrorCostDiffPercent { get; set; }
        public DistributionPoint[] CostDiffDistribution { get; set; } = null!;

        public double MeanCouponYieldYear { get; set; }
        public double StdErrorCouponYieldYear { get; set; }
        public DistributionPoint[] CouponYieldYearDistribution { get; set; } = null!;

        public double MeanTotalYieldYear { get; set; }
        public double StdErrorTotalYieldYear { get; set; }
        public DistributionPoint[] TotalYieldYearDistribution { get; set; } = null!;

        public double MeanDurationYears { get; set; }
        public double StdErrorDurationYears { get; set; }
        public DistributionPoint[] DurationYearsDistribution { get; set; } = null!;
    }
}
