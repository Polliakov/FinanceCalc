using FinanceCalc.Domain.Models.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace FinanceCalc.Persistence.Converters
{
    public class DistributionPointArrayValueConverter : ValueConverter<DistributionPoint[], string>
    {
        public DistributionPointArrayValueConverter()
            : base(
                array => JsonSerializer.Serialize(array, JsonSerializerOptions.Default),
                str => JsonSerializer.Deserialize<DistributionPoint[]>(str, JsonSerializerOptions.Default) 
                       ?? Array.Empty<DistributionPoint>())
        {
        }
    }
}
