using FinanceCalc.Application.Catalog.Repositories.Abstractions;
using FinanceCalc.Domain.Entities;
using FinanceCalc.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceCalc.Persistence.Repositories
{
    public class BondMetricsRepository(AppDbContext db) : IBondMetricsRepository
    {
        private readonly AppDbContext _context = db;

        public async Task AddAsync(BondMetrics metrics, CancellationToken cancellationToken = default)
        {
            var entity = new BondMetricsEntity
            {
                CalculatedAt = DateTime.UtcNow,
                MeanCostDiffPercent = metrics.MeanCostDiffPercent,
                StdErrorCostDiffPercent = metrics.StdErrorCostDiffPercent,
                CostDiffDistribution = metrics.CostDiffDistribution,

                MeanCouponYieldYear = metrics.MeanCouponYieldYear,
                StdErrorCouponYieldYear = metrics.StdErrorCouponYieldYear,
                CouponYieldYearDistribution = metrics.CouponYieldYearDistribution,

                MeanTotalYieldYear = metrics.MeanTotalYieldYear,
                StdErrorTotalYieldYear = metrics.StdErrorTotalYieldYear,
                TotalYieldYearDistribution = metrics.TotalYieldYearDistribution,

                MeanDurationYears = metrics.MeanDurationYears,
                StdErrorDurationYears = metrics.StdErrorDurationYears,
                DurationYearsDistribution = metrics.DurationYearsDistribution,
            };
            await _context.BondsMetrics.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<BondMetrics?> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _context.BondsMetrics.AsNoTracking()
                .OrderByDescending(e => e.CalculatedAt)
                .FirstOrDefaultAsync(cancellationToken);
            if (entity is null) 
                return null;
            return new BondMetrics
            {
                MeanCostDiffPercent = entity.MeanCostDiffPercent,
                StdErrorCostDiffPercent = entity.StdErrorCostDiffPercent,
                CostDiffDistribution = entity.CostDiffDistribution,

                MeanCouponYieldYear = entity.MeanCouponYieldYear,
                StdErrorCouponYieldYear = entity.StdErrorCouponYieldYear,
                CouponYieldYearDistribution = entity.CouponYieldYearDistribution,

                MeanTotalYieldYear = entity.MeanTotalYieldYear,
                StdErrorTotalYieldYear = entity.StdErrorTotalYieldYear,
                TotalYieldYearDistribution = entity.TotalYieldYearDistribution,

                MeanDurationYears = entity.MeanDurationYears,
                StdErrorDurationYears = entity.StdErrorDurationYears,
                DurationYearsDistribution = entity.DurationYearsDistribution,
            };
        }
    }
}
