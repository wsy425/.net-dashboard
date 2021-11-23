using Volo.Abp.Settings;

namespace Dashboard.Settings
{
    public class DashboardSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DashboardSettings.MySetting1));
        }
    }
}
