using FinanceCalc.Domain.Abstractions;

namespace FinanceCalc.Domain.Models.Bonds
{
    public class BondRelevanceResolver : IBondRelevanceResolver
    {
        private static readonly double[] _relevanceWeights =
        [
            -0.10, // CostPercent
            -0.06, // DurationYears
             0.02, // CouponsPerYear
             0.50, // CouponProfitabilityYear
             0.40, // CapitalProfitabilityYear
        ]; // sum = 1.00

        public void CalculateRelevance(IEnumerable<IBond> bonds, BondsMetadata context)
        {
            bonds.ToArray().AsParallel().ForAll(bond =>
            {
                var inputsNormalized = new double[]
                {
                    Math.Abs((double)bond.CostPercent - 1.0),
                    Math.Sqrt(bond.DurationYears / context.DurationYearsMax),
                    bond.CouponsPerYear is null ? 0.0 : bond.CouponsPerYear.Value / 12,
                    (double?)bond.CouponProfitabilityYear / context.CouponProfitabilityYearMax ?? 0.0,
                    (double)bond.CapitalProfitabilityYear / context.CapitalProfitabilityYearMax,
                };
                var relevance = Math.Clamp(inputsNormalized
                    .Select((v, i) => v * _relevanceWeights[i])
                    .Average() * 10, -1, 1);

                bond.Relevance = relevance;
            });
        }
    }
}