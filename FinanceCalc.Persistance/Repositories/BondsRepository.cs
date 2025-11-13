using FinanceCalc.Application.Catalog.Repositories.Abstractions;
using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Entities;
using FinanceCalc.Domain.Exceptions.EntityExceptions;
using FinanceCalc.Domain.Models.Bonds;
using FinanceCalc.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FinanceCalc.Persistence.Repositories
{
    public class BondsRepository(AppDbContext db) : IBondsRepository
    {
        private readonly AppDbContext _context = db;

        public async Task<List<IBond>> GetAllAsync()
        {
            return await _context.Bonds
                .AsNoTracking()
                .ToListAsync<IBond>();
        }

        public async Task<IBond?> GetByIdAsync(string ticker)
        {
            return await _context.Bonds
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Ticker == ticker);
        }

        public async Task AddAsync(IReadOnlyBondData bondData)
        {
            var isExist = await _context.Bonds
                .AnyAsync(b => b.Ticker == bondData.Ticker);
            if (isExist)
                throw new EntityAlreadyExistsException(nameof(BondEntity), bondData.Ticker);

            var bond = new Bond(bondData);
            var entity = BondMapper.ToEntity(bond)
                ?? throw new ArgumentNullException(nameof(bondData));

            await _context.Bonds.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SetRangeAsync(IEnumerable<IReadOnlyBondData> bondsData)
        {
            var bondsDataTickers = bondsData
                .Select(b => b.Ticker)
                .ToArray();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Bonds
                    .Where(b => bondsDataTickers.Contains(b.Ticker))
                    .ExecuteDeleteAsync();

                var entities = bondsData
                    .Select(bondData =>
                    {
                        try
                        {
                            var bond = new Bond(bondData);
                            return BondMapper.ToEntity(bond);
                        }
                        catch
                        {
                            return null;
                        }
                    })
                    .Where(e => e is not null)
                    .Select(e => e!);

                await _context.Bonds.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(IReadOnlyBondData bondData)
        {
            var isExist = await _context.Bonds
                .AnyAsync(b => b.Ticker == bondData.Ticker);
            if (!isExist)
                throw new EntityNotFoundException(nameof(BondEntity), bondData.Ticker);

            var newEntity = BondMapper.ToEntity(new Bond(bondData))!;
            _context.Entry(newEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string ticker)
        {
            var deleted = await _context.Bonds
                .Where(b => b.Ticker == ticker)
                .ExecuteDeleteAsync();

            if (deleted == 0)
                throw new EntityNotFoundException(nameof(BondEntity), ticker);
        }

        public async Task<BondsMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Bonds
                .GroupBy(_ => 1)
                .Select(g => new BondsMetadata
                {
                    DurationYearsMax = g.Max(b => b.DurationYears),
                    CouponProfitabilityYearMax = (double?)g.Max(b => b.CouponProfitabilityYear),
                    CapitalProfitabilityYearMax = (double)g.Max(b => b.CapitalProfitabilityYear)
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
