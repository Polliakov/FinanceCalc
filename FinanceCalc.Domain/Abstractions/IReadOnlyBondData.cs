namespace FinanceCalc.Domain.Abstractions
{
    public interface IReadOnlyBondData
    {
        string Name { get; }
        string Ticker { get; }
        decimal Nominal { get; }
        decimal Cost { get; }
        decimal? Coupon { get; }
        int? CouponsPerYear { get; }
        DateTime? NextCouponDate { get; }
        DateTime DateStart { get; }
        DateTime DateEnd { get; }
        DateTime? OfferDate { get; }
    }
}
