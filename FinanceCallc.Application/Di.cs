using FinanceCalc.Application.Catalog.Services;
using FinanceCalc.Application.Catalog.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Pollyakov.Infrastructure.Abstractions;

namespace FinanceCalc.Application
{
    public class Di : IDependencyInjectionRegister
    {
        public IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IBondsService, BondsService>();
            // репозиторий метрик используется только внутри Persistence, его DI там
            return services;
        }
    }
}
