using FinanceCalc.Domain.Abstractions;

namespace FinanceCalc.Application.Catalog.Repositories.Abstractions
{
    public interface IBondsRepository
    {
        Task AddAsync(IReadOnlyBondData bondData);
        Task DeleteAsync(string ticker);
        Task<List<IBond>> GetAllAsync();
        Task<IBond?> GetByIdAsync(string ticker);
        Task UpdateAsync(IReadOnlyBondData bondData);
        Task SetRangeAsync(IEnumerable<IReadOnlyBondData> bondsData);
    }
}
