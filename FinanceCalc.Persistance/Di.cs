using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pollyakov.Infrastructure.Abstractions;
using FinanceCalc.Application.Catalog.Repositories.Abstractions;
using FinanceCalc.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanceCalc.Persistence
{
    public class Di : IDependencyInjectionRegister
    {
        public IServiceCollection Register(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=FinanceCalc;User Id=FinanceCalcApp;Password=FinanceCalcApp;Include Error Detail=True");
            });
            services.AddTransient<DbInitializer>();

            services.AddScoped<IBondsRepository, BondsRepository>();
            services.AddScoped<IBondMetricsRepository, BondMetricsRepository>();

            return services;
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=FinanceCalc;User Id=FinanceCalcApp;Password=FinanceCalcApp;");
            return new AppDbContext(builder.Options);
        }
    }
}
