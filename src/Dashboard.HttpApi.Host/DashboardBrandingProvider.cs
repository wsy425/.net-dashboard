using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Dashboard
{
    [Dependency(ReplaceServices = true)]
    public class DashboardBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Dashboard";
    }
}
