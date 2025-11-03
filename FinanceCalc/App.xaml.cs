using FinanceCalc.Forms;
using FinanceCalc.Forms.Factory;
using Microsoft.Extensions.DependencyInjection;
using Pollyakov.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FinanceCalc
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var dbInitializer = ServiceProvider.GetRequiredService<Persistence.DbInitializer>();

            try
            {
                await dbInitializer.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Db initialization exception: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown(-1);
                return;
            }

            var startWindow = ServiceProvider.GetRequiredService<MainMenuWindow>();
            startWindow.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainMenuWindow>();
            services.AddTransient<DashboardWindow>();
            services.AddTransient<BondsCatalogWindow>();
            services.AddSingleton<IFormsFactory, FormsFactory>();

            List<IDependencyInjectionRegister> registers =
            [
                new Persistence.Di(),
                new Application.Di(),
                new Sources.Di(),
            ];

            foreach (var register in registers)
            {
                register.Register(services);
            }
        }
    }
}
