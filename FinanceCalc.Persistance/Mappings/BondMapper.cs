using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Entities;

namespace FinanceCalc.Domain.Mappings
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
                CapitalProfitability = bond.CapitalProfitability,
                CapitalProfitabilityYear = bond.CapitalProfitabilityYear,
                ProfitabilityYear = bond.ProfitabilityYear,
                CouponsPeriodMonths = bond.CouponsPeriodMonths,
                CouponProfitability = bond.CouponProfitability,
                CouponProfitabilityYear = bond.CouponProfitabilityYear,
            }
            : null;
        }
    }
}
