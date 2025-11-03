using FinanceCalc.Application.Catalog.DataSources.Abstractions;
using FinanceCalc.Application.Catalog.Repositories.Abstractions;
using FinanceCalc.Application.Extensions.MathNet;
using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models;
using MathNet.Numerics.Statistics;

namespace FinanceCalc.Application.Catalog.Services.Implementations
{
    public class BondsService(
        IBondsRepository bondsRepository,
        IBondsDataSource dataSource,
        IBondMetricsRepository bondMetricsRepository) : IBondsService
    {
        private readonly IBondsRepository _bondsRepository = bondsRepository;
        private readonly IBondsDataSource _dataSource = dataSource;
        private readonly IBondMetricsRepository _bondMetricsRepository = bondMetricsRepository;

        public async Task<IEnumerable<IBond>> GetAllAsync() => await _bondsRepository.GetAllAsync();
        public async Task<IBond?> GetByIdAsync(string ticker) => await _bondsRepository.GetByIdAsync(ticker);
        public async Task AddAsync(IReadOnlyBondData bond) => await _bondsRepository.AddAsync(bond);
        public async Task UpdateAsync(IReadOnlyBondData bond) => await _bondsRepository.UpdateAsync(bond);
        public async Task DeleteAsync(string ticker) => await _bondsRepository.DeleteAsync(ticker);

        public async Task<int> ImportFromMoex(CancellationToken cancellationToken = default)
        {
            var fetched = await _dataSource.FetchAsync(cancellationToken);
            int count = fetched.Count();
            await _bondsRepository.SetRangeAsync(fetched);
            return count;
        }

        public async Task<BondMetrics> CalculateMetricsAsync(int precision, CancellationToken cancellationToken = default)
        {
            var bonds = await _bondsRepository.GetAllAsync();
            var nominalDiffPercent = bonds.Select(b => (double)(b.Cost / b.Nominal)).ToArray();
            var couponYields = bonds
                 .Where(b => b.CouponProfitabilityYear.HasValue)
                 .Select(b => (double)b.CouponProfitabilityYear!.Value)
                 .ToArray();
            var totalYields = bonds
                .Select(b => (double)b.ProfitabilityYear)
                .ToArray();
            var durations = bonds
                .Select(b => b.DurationYears)
                .ToArray();

            var metrics = new BondMetrics
            {
                MeanCostDiffPercent = nominalDiffPercent.Mean(),
                StdErrorCostDiffPercent = nominalDiffPercent.StandardDeviation(),
                CostDiffDistribution = new Histogram(nominalDiffPercent, precision).GetDistribution(),

                MeanCouponYieldYear = couponYields.Mean(),
                StdErrorCouponYieldYear = couponYields.StandardDeviation(),
                CouponYieldYearDistribution = new Histogram(couponYields, precision).GetDistribution(),

                MeanTotalYieldYear = totalYields.Mean(),
                StdErrorTotalYieldYear = totalYields.StandardDeviation(),
                TotalYieldYearDistribution = new Histogram(totalYields, precision).GetDistribution(),

                MeanDurationYears = durations.Mean(),
                StdErrorDurationYears = durations.StandardDeviation(),
                DurationYearsDistribution = new Histogram(durations, precision).GetDistribution(),
            };

            return metrics;
        }

        public async Task SaveMetricsAsync(BondMetrics metrics, CancellationToken cancellationToken = default)
        {
            await _bondMetricsRepository.AddAsync(metrics, cancellationToken);
        }

        public async Task<BondMetrics?> GetLastMetricsAsync(CancellationToken cancellationToken = default)
        {
            return await _bondMetricsRepository.GetLastAsync(cancellationToken);
        }
    }
}
