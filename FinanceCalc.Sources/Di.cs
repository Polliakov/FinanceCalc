using FinanceCalc.Application.Catalog.DataSources.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Pollyakov.Infrastructure.Abstractions;

namespace FinanceCalc.Sources
{
    public class Di : IDependencyInjectionRegister
    {
        public IServiceCollection Register(IServiceCollection services)
        {
            services.AddTransient<IBondsDataSource, Moex.BondsDataSource>();
            services.AddHttpClient(Moex.BondsDataSource.HttpClientName, c =>
            {
                c.BaseAddress = new Uri("https://iss.moex.com/iss/");
            });
            return services;
        }
    }
}
