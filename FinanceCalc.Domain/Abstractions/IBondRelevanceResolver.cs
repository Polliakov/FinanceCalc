using FinanceCalc.Domain.Models.Bonds;

namespace FinanceCalc.Domain.Abstractions
{
    public interface IBondRelevanceResolver
    {
        void CalculateRelevance(IEnumerable<IBond> bonds, BondsMetadata context);
    }
}