using Abp.Authorization;
using Abp.Localization;

namespace WebDemo.Providers
{
    public class WebDemoAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages);
            if (pages == null)
            {
                pages = context.CreatePermission(PermissionNames.Pages, L("Pages"));
                //pages.CreateChildPermission
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WebDemoConsts.LocalizationSourceName);
        }
    }
}
