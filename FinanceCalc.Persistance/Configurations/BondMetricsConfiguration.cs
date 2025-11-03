using FinanceCalc.Domain.Entities;
using FinanceCalc.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceCalc.Persistence.Configurations
{
    internal class BondMetricsConfiguration : IEntityTypeConfiguration<BondMetricsEntity>
    {
        public void Configure(EntityTypeBuilder<BondMetricsEntity> builder)
        {
            builder.ToTable("BondMetrics");

            builder.HasKey(b => b.CalculatedAt);

            builder
                .Property(b => b.MeanCostDiffPercent)
                .IsRequired();
            builder
                .Property(b => b.StdErrorCostDiffPercent)
                .IsRequired();
            builder
                .Property(b => b.CostDiffDistribution)
                .HasConversion(new DistributionPointArrayValueConverter())
                .IsRequired();

            builder
                .Property(b => b.MeanCouponYieldYear)
                .IsRequired();
            builder
                .Property(b => b.StdErrorCouponYieldYear)
                .IsRequired();
            builder
                .Property(b => b.CouponYieldYearDistribution)
                .HasConversion(new DistributionPointArrayValueConverter())
                .IsRequired();

            builder
                .Property(b => b.MeanTotalYieldYear)
                .IsRequired();
            builder
                .Property(b => b.StdErrorTotalYieldYear)
                .IsRequired();
            builder
                .Property(b => b.TotalYieldYearDistribution)
                .HasConversion(new DistributionPointArrayValueConverter())
                .IsRequired();

            builder
               .Property(b => b.MeanDurationYears)
               .IsRequired();
            builder
                .Property(b => b.StdErrorDurationYears)
                .IsRequired();
            builder
                .Property(b => b.DurationYearsDistribution)
                .HasConversion(new DistributionPointArrayValueConverter())
                .IsRequired();
        }
    }
}
