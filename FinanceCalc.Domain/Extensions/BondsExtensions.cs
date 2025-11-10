using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models.Bonds;

namespace FinanceCalc.Domain.Extensions
{
    public static class BondsExtensions
    {
        public static IBondData AsMutable(this IReadOnlyBondData bond)
        {
            return new BondData
            {
                Name = bond.Name,
                Ticker = bond.Ticker,
                Nominal = bond.Nominal,
                Cost = bond.Cost,
                Coupon = bond.Coupon,
                CouponsPerYear = bond.CouponsPerYear,
                DateStart = bond.DateStart,
                DateEnd = bond.DateEnd
            };
        }
    }
}
