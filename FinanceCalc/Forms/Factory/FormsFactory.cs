using Microsoft.Extensions.DependencyInjection;

namespace FinanceCalc.Forms.Factory
{
    public class FormsFactory(IServiceProvider serviceProvider) : IFormsFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public BondsCatalogWindow CreateBondsCatalogWindow()
        {
            return _serviceProvider.GetRequiredService<BondsCatalogWindow>();
        }

        public DashboardWindow CreateDashboardWindow()
        {
            return _serviceProvider.GetRequiredService<DashboardWindow>();
        }
    }
}
