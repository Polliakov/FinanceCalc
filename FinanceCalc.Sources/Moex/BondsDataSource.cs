using FinanceCalc.Application.Catalog.DataSources.Abstractions;
using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Models;
using System.Globalization;
using System.Text.Json;

namespace FinanceCalc.Sources.Moex
{
    public class BondsDataSource(IHttpClientFactory httpClientFactory) : IBondsDataSource
    {
        public const string HttpClientName = "MoexBonds";
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(HttpClientName);

        public async Task<IEnumerable<IReadOnlyBondData>> FetchAsync(CancellationToken cancellationToken = default)
        {
            var requestUrl = "engines/stock/markets/bonds/boards/TQCB/securities.json?iss.meta=off&iss.only=securities,marketdata" +
                             "&securities.columns=SECID,SHORTNAME,NAME,ISSUER_NAME,FACEVALUE,MATDATE,COUPONVALUE,COUPONPERIOD,OFFERDATE" +
                             "&marketdata.columns=SECID,LAST";

            using var response = await _httpClient.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);
            using var doc = await JsonDocument
                .ParseAsync(responseStream, cancellationToken: cancellationToken).ConfigureAwait(false);

            var root = doc.RootElement;
            if (!root.TryGetProperty("securities", out var data))
                return [];
            if (!root.TryGetProperty("marketdata", out var prices))
                return [];

            var dataColumns = data.GetProperty("columns").EnumerateArray()
                .Select((e, i) => (e.GetString()!, i))
                .ToDictionary(t => t.Item1, t => t.i);
            var priceColumns = prices.GetProperty("columns").EnumerateArray()
                .Select((e, i) => (e.GetString()!, i))
                .ToDictionary(t => t.Item1, t => t.i);

            var pricesRows = prices.GetProperty("data").EnumerateArray()
                .Select(e => e.EnumerateArray().Select(je => je.ToString()).ToArray())
                .ToDictionary(rv => rv[priceColumns["SECID"]]);

            var bonds = new List<IReadOnlyBondData>();
            var fallbackList = new List<BondData>();
            foreach (var row in data.GetProperty("data").EnumerateArray())
            {
                cancellationToken.ThrowIfCancellationRequested();

                var values = row.EnumerateArray().Select(v => v.ToString()).ToArray();
                string ticker = Get(values, dataColumns, "SECID");
                if (string.IsNullOrWhiteSpace(ticker))
                    continue;

                var shortName = Get(values, dataColumns, "SHORTNAME");
                var fullName = Get(values, dataColumns, "NAME");
                var issuerName = Get(values, dataColumns, "ISSUER_NAME");

                var nominal = ParseDecimal(Get(values, dataColumns, "FACEVALUE"));
                var coupon = ParseNullableDecimal(Get(values, dataColumns, "COUPONVALUE"));
                var couponPeriodDays = ParseNullableInt(Get(values, dataColumns, "COUPONPERIOD"));
                var endDate = ParseDate(Get(values, dataColumns, "MATDATE"));
                var offerDate = ParseDate(Get(values, dataColumns, "OFFERDATE"));
                var lastTradePriceRate = 1m;
                if (pricesRows.TryGetValue(ticker, out var mdr))
                {
                    lastTradePriceRate = ParseDecimal(mdr[priceColumns["LAST"]], 100) / 100;
                }
                int? couponsPerYear = couponPeriodDays is > 0 ? (int)Math.Round(365m / couponPeriodDays.Value) : null;

                var displayName = !string.IsNullOrWhiteSpace(fullName) ? fullName :
                                   !string.IsNullOrWhiteSpace(shortName) ? shortName :
                                   !string.IsNullOrWhiteSpace(issuerName) ? issuerName : null;

                var bond = new BondData
                {
                    Ticker = ticker,
                    Name = displayName ?? "No Name",
                    Nominal = nominal,
                    Cost = lastTradePriceRate * nominal,
                    Coupon = coupon,
                    CouponsPerYear = couponsPerYear,
                    DateStart = DateTime.UtcNow,
                    DateEnd = endDate ?? DateTime.UtcNow.AddYears(1),
                    OfferDate = offerDate,
                };
                bonds.Add(bond);

                if (displayName is null)
                {
                    fallbackList.Add(bond);
                }
            }

            foreach (var item in fallbackList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var detailUrl = $"securities/{item.Ticker}.json?iss.meta=off&iss.only=description";
                    using var detailResponse = await _httpClient.GetAsync(detailUrl, cancellationToken).ConfigureAwait(false);
                    if (!detailResponse.IsSuccessStatusCode) 
                        continue;
                    
                    await using var detailStream = await detailResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
                    using var detailDoc = await JsonDocument.ParseAsync(detailStream, cancellationToken: cancellationToken).ConfigureAwait(false);
                    if (!detailDoc.RootElement.TryGetProperty("description", out var description)) 
                        continue;
                    if (!description.TryGetProperty("data", out var rows)) 
                        continue;

                    foreach (var row in rows.EnumerateArray())
                    {
                        var arr = row.EnumerateArray().Select(x => x.ToString()).ToArray();
                        if (arr.Length < 2) 
                            continue;
                        var key = arr[0];
                        var value = arr[1];
                        if (key == "NAME" || key == "SECNAME" || key == "EMITENT_TITLE")
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                item.Name = value;
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    // ignore single security enrichment errors
                }
            }

            return bonds;
        }

        private static string Get(string[] arr, Dictionary<string, int> map, string key) => 
            map.TryGetValue(key, out var i) && i < arr.Length ? arr[i] : string.Empty;
        private static decimal ParseDecimal(string s, decimal defaultValue = 0m) => 
            decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : defaultValue;
        private static decimal? ParseNullableDecimal(string s) => 
            decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : null;
        private static int? ParseNullableInt(string s) => 
            int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var n) ? n : null;
        private static DateTime? ParseDate(string s) => 
            DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt) ? dt.ToUniversalTime() : null;
    }
}
