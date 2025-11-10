using FinanceCalc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FinanceCalc.Persistence.Extensions;
using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BondEntity> Bonds => Set<BondEntity>();
        public DbSet<BondMetricsEntity> BondsMetrics => Set<BondMetricsEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.Ignore<DistributionPoint>();
            modelBuilder.ApplyDecimalHardRounding();
        }
    }
}
