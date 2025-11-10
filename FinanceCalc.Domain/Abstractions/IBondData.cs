namespace FinanceCalc.Domain.Abstractions
{
    public interface IBondData : IReadOnlyBondData
    {
        new string Name { get; set; }
        new string Ticker { get; set; }
        new decimal Nominal { get; set; }
        new decimal Cost { get; set; }
        new decimal? Coupon { get; set; }
        new int? CouponsPerYear { get; set; }
        new DateTime? NextCouponDate { get; set; }
        new DateTime DateStart { get; set; }
        new DateTime DateEnd { get; set; }
        new DateTime? OfferDate { get; set; }
        new bool NeedQualification { get; set; }
    }
}
