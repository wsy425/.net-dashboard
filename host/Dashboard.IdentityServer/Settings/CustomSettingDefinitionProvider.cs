using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Dashboard.Settings
{
    public class CustomSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(IdentitySettingNames.Password.RequireDigit,
                    CustomPasswordSettings.RequireDigit.ToString()),
                new SettingDefinition(IdentitySettingNames.Password.RequiredLength,
                    CustomPasswordSettings.RequiredLength.ToString()),
                new SettingDefinition(IdentitySettingNames.Password.RequireLowercase,
                    CustomPasswordSettings.RequireLowercase.ToString()),
                new SettingDefinition(IdentitySettingNames.Password.RequireUppercase,
                    CustomPasswordSettings.RequireUppercase.ToString()),
                new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars,
                    CustomPasswordSettings.RequiredUniqueChars.ToString()),
                new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric,
                    CustomPasswordSettings.RequireNonAlphanumeric.ToString())
            );
            var all = context.GetAll();
            var aa = context.GetAll();
            
        }
    }
}