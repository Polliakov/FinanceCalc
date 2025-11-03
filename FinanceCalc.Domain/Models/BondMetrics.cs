using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Domain.Models
{
    public class BondMetrics
    {
        public double MeanCostDiffPercent { get; set; }
        public double StdErrorCostDiffPercent { get; set; }
        public DistributionPoint[] CostDiffDistribution { get; set; } = [];

        public double MeanCouponYieldYear { get; set; }
        public double StdErrorCouponYieldYear { get; set; }
        public DistributionPoint[] CouponYieldYearDistribution { get; set; } = [];

        public double MeanTotalYieldYear { get; set; }
        public double StdErrorTotalYieldYear { get; set; }
        public DistributionPoint[] TotalYieldYearDistribution { get; set; } = [];

        public double MeanDurationYears { get; set; }
        public double StdErrorDurationYears { get; set; }
        public DistributionPoint[] DurationYearsDistribution { get; set; } = [];
    }
}
