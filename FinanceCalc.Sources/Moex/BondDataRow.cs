namespace FinanceCalc.Sources.Moex
{
    public class BondDataRow
    {
        public string SecId { get; set; } = null!;
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? IssuerName { get; set; }
        public decimal FaceValue { get; set; }
        public decimal? PrevPrice { get; set; }
        public decimal? CouponValue { get; set; }
        public double? CouponPeriod { get; set; }
        public DateTime? NextCoupon { get; set; }
        public DateTime? MatDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public bool? QualifiedRequired { get; set; }
    }
}
