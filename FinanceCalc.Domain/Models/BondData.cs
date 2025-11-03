using FinanceCalc.Domain.Abstractions;

namespace FinanceCalc.Domain.Models
{
    public class BondData : IBondData
    {
        public required string Name { get; set; }
        public required string Ticker { get; set; }
        public required decimal Nominal { get; set; }
        public required decimal Cost { get; set; }
        public decimal? Coupon { get; set; }
        public int? CouponsPerYear { get; set; }
        public required DateTime DateStart { get; set; }
        public required DateTime DateEnd { get; set; }
        public DateTime? OfferDate { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
