namespace FinanceCalc.Forms.Factory
{
    public interface IFormsFactory
    {
        BondsCatalogWindow CreateBondsCatalogWindow();
        DashboardWindow CreateDashboardWindow();
    }
}