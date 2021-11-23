using System;
using System.Collections.Generic;
using System.Text;
using Dashboard.Localization;
using Volo.Abp.Application.Services;

namespace Dashboard
{
    /* Inherit your application services from this class.
     */
    public abstract class DashboardAppService : ApplicationService
    {
        protected DashboardAppService()
        {
            LocalizationResource = typeof(DashboardResource);
        }
    }
}
