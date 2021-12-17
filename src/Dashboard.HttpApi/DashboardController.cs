using Dashboard.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Dashboard
{
    public abstract class DashboardController : AbpController
    {
        protected DashboardController()
        {
            LocalizationResource = typeof(DashboardResource);
        }
    }
}
