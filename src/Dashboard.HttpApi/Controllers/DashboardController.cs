using Dashboard.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class DashboardController : AbpController
    {
        protected DashboardController()
        {
            LocalizationResource = typeof(DashboardResource);
        }
    }
}