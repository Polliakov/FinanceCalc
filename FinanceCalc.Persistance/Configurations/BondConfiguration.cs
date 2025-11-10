using FinanceCalc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceCalc.Persistence.Configurations
{
    internal class BondConfiguration : IEntityTypeConfiguration<BondEntity>
    {
        public void Configure(EntityTypeBuilder<BondEntity> builder)
        {
            builder.ToTable("Bonds");

            builder.HasKey(b => new { b.Ticker, b.DateStart });

            builder
                .Property(b => b.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder
                .Property(b => b.Ticker)
                .HasMaxLength(64)
                .IsRequired();

            builder
                .Property(b => b.Nominal)
                .HasPrecision(16, 6)
                .IsRequired();

            builder
                .Property(b => b.Cost)
                .HasPrecision(16, 6)
                .IsRequired();

            builder
                .Property(b => b.Coupon)
                .HasPrecision(16, 6)
                .IsRequired(false);

            builder
                .Property(b => b.AccumulatedCouponIncome)
                .HasPrecision(16, 6)
                .IsRequired(false);

            builder
                .Property(b => b.CouponsPerYear)
                .IsRequired(false);

            builder
                .Property(b => b.NextCouponDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            builder
                .Property(b => b.DateStart)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder
                .Property(b => b.DateEnd)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder
                .Property(b => b.OfferDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            builder
                .Property(b => b.DurationYears)
                .IsRequired();

            builder
                .Property(b => b.CouponsPeriodMonths)
                .IsRequired(false);

            builder
                .Property(b => b.CouponProfitability)
                .HasPrecision(10, 5)
                .IsRequired(false);

            builder
                .Property(b => b.CouponProfitabilityYear)
                .HasPrecision(10, 5)
                .IsRequired(false);

            builder
                .Property(b => b.CapitalProfitability)
                .HasPrecision(10, 5)
                .IsRequired();

            builder
                .Property(b => b.CapitalProfitabilityYear)
                .HasPrecision(10, 5)
                .IsRequired();

            builder
                .Property(b => b.ProfitabilityYear)
                .HasPrecision(10, 5)
                .IsRequired();

            builder
                .Property(b => b.NeedQualification)
                .IsRequired();
        }
    }
}
