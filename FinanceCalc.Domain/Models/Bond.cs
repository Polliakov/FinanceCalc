using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Domain.Models
{
    public class Bond : IBond
    {
        public Bond() : this(new BondData
        {
            Name = "Селигдар 001Р-03",
            Ticker = "RU000A10B933",
            Cost = 1097.4m,
            Nominal = 1000,
            Coupon = 19.11m,
            CouponsPerYear = 12,
            DateStart = DateTime.Parse("2025-10-22"),
            DateEnd = DateTime.Parse("2027-09-25"),
        })
        { }

        public Bond(IReadOnlyBondData data)
        {
            Name = data.Name;
            Ticker = data.Ticker;
            Nominal = data.Nominal;
            Cost = data.Cost;
            DateStart = data.DateStart;
            DateEnd = data.DateEnd;
            DurationYears = Math.Max(0, (DateEnd - DateStart).TotalDays / 365);
            OfferDate = data.OfferDate;
            NextCouponDate = data.NextCouponDate;

            if (Cost > 0 &&
                data.Coupon is { } coupon && coupon > 0 &&
                data.CouponsPerYear is { } couponsPerYear && couponsPerYear > 0)
            {

                Coupon = coupon;
                CouponsPerYear = couponsPerYear;
                CouponsPeriodMonths = 12.0 / couponsPerYear;
                CouponProfitability = new ComplexPercent(coupon / Cost, CouponsPeriodMonths.Value);
                CouponProfitabilityYear = CouponProfitability.WithPeriod(12);
                if (NextCouponDate is not null)
                {
                    var couponsPeriodDays = 365.0 / couponsPerYear;
                    // Dummy year rate for accumulated income calculation.
                    AccumulatedCouponIncome = Coupon * 
                        (decimal)((DateTime.UtcNow.Date -
                            NextCouponDate!.Value.Date.AddDays(-couponsPeriodDays)).TotalDays
                        / couponsPeriodDays);
                }
            }
            if (Cost > 0)
            {
                CapitalProfitability = new ComplexPercent((Nominal - Cost) / Cost, (double)DurationYears);
                CapitalProfitabilityYear = CapitalProfitability.WithPeriod(1).SetPeriod(12);
            }
            else
            {
                CapitalProfitability = ComplexPercent.Zero.SetPeriod(DurationYears);
                CapitalProfitabilityYear = ComplexPercent.Zero.SetPeriod(12);
            }
            ProfitabilityYear = (CouponProfitabilityYear ?? ComplexPercent.Zero.SetPeriod(12)) + CapitalProfitabilityYear;
        }

        public string Name { get; private set; }
        public string Ticker { get; private set; }
        public decimal Nominal { get; private set; }
        public decimal Cost { get; private set; }
        public decimal? Coupon { get; private set; }
        public decimal? AccumulatedCouponIncome { get; private set; }
        public int? CouponsPerYear { get; private set; }
        public DateTime? NextCouponDate { get; private set; }
        public DateTime DateStart { get; private set; }
        public DateTime DateEnd { get; private set; }
        public DateTime? OfferDate { get; private set; }
        public double DurationYears { get; private set; }
        public double? CouponsPeriodMonths { get; private set; }
        public ComplexPercent? CouponProfitability { get; private set; }
        public ComplexPercent? CouponProfitabilityYear { get; private set; }
        public ComplexPercent CapitalProfitability { get; private set; }
        public ComplexPercent CapitalProfitabilityYear { get; private set; }
        public ComplexPercent ProfitabilityYear { get; private set; }

        decimal? IBond.AccumulatedCouponIncome => AccumulatedCouponIncome!;
        decimal? IBond.CouponProfitability => CouponProfitability!;
        decimal? IBond.CouponProfitabilityYear => CouponProfitabilityYear!;
        decimal IBond.CapitalProfitability => CapitalProfitability;
        decimal IBond.CapitalProfitabilityYear => CapitalProfitabilityYear;
        decimal IBond.ProfitabilityYear => ProfitabilityYear;

        public object Clone()
        {
            return new Bond(new BondData
            {
                Name = Name,
                Ticker = Ticker,
                Nominal = Nominal,
                Cost = Cost,
                Coupon = Coupon,
                CouponsPerYear = CouponsPerYear,
                NextCouponDate = NextCouponDate,
                DateStart = DateStart,
                DateEnd = DateEnd,
                OfferDate = OfferDate,
            });
        }
    }
}
