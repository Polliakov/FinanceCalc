using FinanceCalc.Application.Catalog.DataSources.Abstractions;
using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models.Bonds;
using System.Text.Json;

namespace FinanceCalc.Sources.Moex
{
    public class BondsDataSource(IHttpClientFactory httpClientFactory) : IBondsDataSource
    {
        public const string HttpClientName = "MoexBonds";
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(HttpClientName);

        private const string _bondsRequest = "engines/stock/markets/bonds/boards/TQCB/securities.json" +
            "?iss.meta=off&iss.only=securities" +
            "&securities.columns=SECID,SHORTNAME,NAME,ISSUER_NAME," +
            "FACEVALUE,PREVPRICE," +
            "MATDATE,OFFERDATE," +
            "COUPONVALUE,COUPONPERIOD,NEXTCOUPON";

        public async Task<IEnumerable<IReadOnlyBondData>> FetchAsync(CancellationToken cancellationToken = default)
        {
            var dataRows = await FetchBondsData(cancellationToken);

            var bondData = new List<IReadOnlyBondData>();
            foreach (var row in dataRows)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var displayName = !string.IsNullOrWhiteSpace(row.Name) ? row.Name :
                                  !string.IsNullOrWhiteSpace(row.ShortName) ? row.ShortName :
                                  !string.IsNullOrWhiteSpace(row.IssuerName) ? row.IssuerName : null;
                int? couponsPerYear = row.CouponPeriod is > 0 ?
                    (int)Math.Round(365 / row.CouponPeriod.Value) : null;

                var bond = new BondData
                {
                    Ticker = row.SecId,
                    Name = displayName ?? "No Name",
                    Nominal = row.FaceValue,
                    Cost = row.PrevPrice is null ?
                        row.FaceValue :
                        row.PrevPrice.Value / 100 * row.FaceValue,
                    Coupon = row.CouponValue,
                    CouponsPerYear = couponsPerYear,
                    NextCouponDate = row.NextCoupon,
                    DateStart = DateTime.UtcNow,
                    DateEnd = row.MatDate ?? DateTime.UtcNow.AddYears(1),
                    OfferDate = row.OfferDate,
                    NeedQualification = row.QualifiedRequired ?? false,
                };
                bondData.Add(bond);
            }

            return bondData;
        }

        private async Task<BondDataRow[]> FetchBondsData(CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(_bondsRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(responseStream, cancellationToken: cancellationToken);

            var root = document.RootElement;
            if (!root.TryGetProperty("securities", out var securities))
                return [];

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new DateTimeJsonConverter(),
                }
            };
            var mappings = securities.GetProperty("columns").EnumerateArray()
                .Select((e, i) => new { Name = e.GetString()!, Index = i })
                .ToDictionary(e => e.Name, e => e.Index);
            var dataRowsRaw = securities.GetProperty("data").EnumerateArray()
                .Select(row => row.EnumerateArray()
                    .Select((column, i) => new { Value = column, Index = i })
                    .Join(mappings, c => c.Index, m => m.Value,
                        (c, m) => new KeyValuePair<string, JsonElement>(m.Key, c.Value))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                .ToList();
            var dataRows = dataRowsRaw
                .Select(row => JsonSerializer.SerializeToElement(row, jsonOptions)
                    .Deserialize<BondDataRow>(jsonOptions)!)
                .ToArray();
            return dataRows;
        }
    }
}
