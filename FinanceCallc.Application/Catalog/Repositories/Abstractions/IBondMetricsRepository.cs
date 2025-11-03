using FinanceCalc.Domain.Models;

namespace FinanceCalc.Application.Catalog.Repositories.Abstractions
{
    public interface IBondMetricsRepository
    {
        Task AddAsync(BondMetrics metrics, CancellationToken cancellationToken = default);
        Task<BondMetrics?> GetLastAsync(CancellationToken cancellationToken = default);
    }
}
