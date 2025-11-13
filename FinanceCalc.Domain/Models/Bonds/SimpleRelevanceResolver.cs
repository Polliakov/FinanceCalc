using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Domain.Models.Bonds
{
    public class SimpleRelevanceResolver : IBondRelevanceResolver
    {
        private static readonly double[] _relevanceWeights =
        [
            -0.10, // CostPercent
            -0.005,// DurationYears
            -0.05, // DurationPenalty
             0.01, // CouponsPerYear
             0.50, // CouponProfitabilityYear
             0.40, // CapitalProfitabilityYear
        ];
        private static readonly double _durationPenaltyThreshold = DoubleTime.Months(1);

        public void CalculateRelevance(IEnumerable<IBond> bonds, BondsMetadata context)
        {
            bonds.ToArray().AsParallel().ForAll(bond =>
            {
                var inputsNormalized = new double[]
                {
                    Math.Abs((double)bond.CostPercent - 1.0),
                    bond.DurationYears,
                    // dy = 1 month penalty = 1
                    // lim(penalty, dy->0) = infinity
                    Math.Abs(DoubleTime.Month / bond.DurationYears),
                    bond.CouponsPerYear is null ? 
                        0.0 : bond.CouponsPerYear.Value / DoubleTime.MonthsInYear,
                    (double?)bond.CouponProfitabilityYear ?? 0.0,
                    (double)bond.CapitalProfitabilityYear,
                };
                var relevance = Math.Clamp(inputsNormalized
                    .Select((v, i) => v * _relevanceWeights[i])
                    .Sum(), -1, 1);

                bond.Relevance = relevance;
            });
        }
    }
}