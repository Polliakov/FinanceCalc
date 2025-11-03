using FinanceCalc.Domain.Abstractions;

namespace FinanceCalc.Application.Catalog.DataSources.Abstractions
{
    public interface IBondsDataSource
    {
        Task<IEnumerable<IReadOnlyBondData>> FetchAsync(CancellationToken cancellationToken = default);
    }
}
