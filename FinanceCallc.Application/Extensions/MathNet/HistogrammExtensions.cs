using FinanceCalc.Domain.Models.Primitives;
using MathNet.Numerics.Statistics;

namespace FinanceCalc.Application.Extensions.MathNet
{
    public static class HistogramExtensions
    {
        public static DistributionPoint[] GetDistribution(this Histogram histogram)
        {
            var points = new DistributionPoint[histogram.BucketCount];
            for (int i = 0; i < histogram.BucketCount; i++)
            {
                points[i] = new DistributionPoint
                {
                    LowerBound = histogram[i].LowerBound,
                    UpperBound = histogram[i].UpperBound,
                    Count = (int)histogram[i].Count
                };
            }
            return points;
        }
    }
}
