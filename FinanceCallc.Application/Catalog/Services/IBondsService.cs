using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models;

namespace FinanceCalc.Application.Catalog.Services
{
    public interface IBondsService
    {
        Task AddAsync(IReadOnlyBondData bond);
        Task DeleteAsync(string ticker);
        Task<IEnumerable<IBond>> GetAllAsync();
        Task<IBond?> GetByIdAsync(string ticker);
        Task UpdateAsync(IReadOnlyBondData bond);
        Task<int> ImportFromMoex(CancellationToken cancellationToken = default);
        Task<BondMetrics> CalculateMetricsAsync(int precision, CancellationToken cancellationToken = default);
        Task SaveMetricsAsync(BondMetrics metrics, CancellationToken cancellationToken = default);
        Task<BondMetrics?> GetLastMetricsAsync(CancellationToken cancellationToken = default);
    }
}