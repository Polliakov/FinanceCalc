using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models.Bonds;

namespace FinanceCalc.Domain.Entities
{
    public class BondEntity : IBond
    {
        public required string Name { get; set; }
        public required string Ticker { get; set; }
        public required decimal Nominal { get; set; }
        public required decimal Cost { get; set; }
        public decimal? Coupon { get; set; }
        public decimal? AccumulatedCouponIncome { get; set; }
        public int? CouponsPerYear { get; set; }
        public DateTime? NextCouponDate { get; set; }
        public required DateTime DateStart { get; set; }
        public required DateTime DateEnd { get; set; }
        public DateTime? OfferDate { get; set; }
        public required double DurationYears { get; set; }
        public double? CouponsPeriodMonths { get; set; }
        public decimal? CouponProfitability { get; set; }
        public decimal? CouponProfitabilityYear { get; set; }
        public required decimal CapitalProfitability { get; set; }
        public required decimal CapitalProfitabilityYear { get; set; }
        public required decimal ProfitabilityYear { get; set; }
        public double? Relevance { get; set; }
        public bool NeedQualification { get; set; }

        // For view. Not stored in DB.
        public decimal CostPercent => Nominal <= 0 ? 1 : Cost / Nominal;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
