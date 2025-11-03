namespace FinanceCalc.Domain.Models.Primitives
{
    public class DistributionPoint
    {
        public required double LowerBound { get; set; }
        public required double UpperBound { get; set; }
        public required int Count { get; set; }

        public override string ToString()
        {
            return $"{LowerBound} - {UpperBound}: {Count}";
        }
    }
}
