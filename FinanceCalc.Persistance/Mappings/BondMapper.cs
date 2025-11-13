using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Entities;
using FinanceCalc.Domain.Models.Bonds;

namespace FinanceCalc.Persistence.Mappings
{
    public static class BondMapper
    {
        public static BondEntity? ToEntity(IBond bond)
        {
            return bond is not null ? bond as BondEntity ?? new BondEntity
            {
                Name = bond.Name,
                Ticker = bond.Ticker,
                Nominal = bond.Nominal,
                Cost = bond.Cost,
                Coupon = bond.Coupon,
                AccumulatedCouponIncome = bond.AccumulatedCouponIncome,
                CouponsPerYear = bond.CouponsPerYear,
                NextCouponDate = bond.NextCouponDate,
                DateStart = bond.DateStart,
                DateEnd = bond.DateEnd,
                OfferDate = bond.OfferDate,
                DurationYears = bond.DurationYears,
                CapitalProfitability = TruncateDecimal(bond.CapitalProfitability, 5),
                CapitalProfitabilityYear = TruncateDecimal(bond.CapitalProfitabilityYear, 5),
                ProfitabilityYear = TruncateDecimal(bond.ProfitabilityYear, 5),
                CouponsPeriodMonths = bond.CouponsPeriodMonths,
                CouponProfitability = TruncateDecimal(bond.CouponProfitability, 5),
                CouponProfitabilityYear = TruncateDecimal(bond.CouponProfitabilityYear, 5),
                Relevance = bond.Relevance,
                NeedQualification = bond.NeedQualification,
            }
            : null;
        }

        public static Bond ToDomain(IBondData data)
        {
            return new Bond(data);
        }

        private static decimal TruncateDecimal(decimal value, int valuePrecision)
        {
            var factor = (decimal)Math.Pow(10, valuePrecision) - 1;
            return Math.Clamp(value, -factor, factor);
        }

        private static decimal? TruncateDecimal(decimal? value, int valuePrecision)
        {
            if (value is null)
                return null;
            var factor = (decimal)Math.Pow(10, valuePrecision) - 1;
            return Math.Clamp(value.Value, -factor, factor);
        }
    }
}
