using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceCalc.Persistence.Extensions
{
    public static class ModelBuilderPrecisionExtensions
    {
        public static ModelBuilder ApplyDecimalHardRounding(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var entityBuilder = modelBuilder.Entity(entityType.ClrType);
                foreach (var property in entityType.GetProperties())
                {
                    if (property.GetValueConverter() is not null)
                        continue;

                    if (property.ClrType != typeof(decimal) && property.ClrType != typeof(decimal?))
                        continue;

                    var precision = property.GetPrecision();
                    var scale = property.GetScale();
                    if (!precision.HasValue || !scale.HasValue)
                        continue;

                    if (property.IsNullable)
                    {
                        var converter = new ValueConverter<decimal?, decimal?>(
                            v => v.HasValue
                                ? decimal.Round(v.Value, scale.Value, MidpointRounding.ToZero)
                                : v,
                            v => v.HasValue
                                ? decimal.Round(v.Value, scale.Value, MidpointRounding.ToZero)
                                : v);
                        entityBuilder
                            .Property(property.ClrType, property.Name)
                            .HasConversion(converter);
                    }
                    else
                    {
                        var converter = new ValueConverter<decimal, decimal>(
                        v => decimal.Round(v, scale.Value, MidpointRounding.ToZero),
                            v => decimal.Round(v, scale.Value, MidpointRounding.ToZero));
                        entityBuilder
                            .Property(property.ClrType, property.Name)
                            .HasConversion(converter);
                    }
                }
            }
            return modelBuilder;
        }
    }
}
