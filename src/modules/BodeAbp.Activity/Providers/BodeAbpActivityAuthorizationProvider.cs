using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;

namespace BodeAbp.Activity.Providers
{
    public class BodeAbpActivityAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var activity = context.GetPermissionOrNull(PermissionNames.Activity);
            if (activity == null)
            {
                activity = context.CreatePermission(PermissionNames.Activity, L(PermissionNames.Activity));
                
                //活动
                var activityActivitys = activity.CreateChildPermission(PermissionNames.Activity_Activity, L(PermissionNames.Activity_Activity));
                activityActivitys.CreateChildPermission(PermissionNames.Activity_Activity_Crud, L(PermissionNames.Activity_Activity_Crud));

                //活动类型
                var activityClassifys = activity.CreateChildPermission(PermissionNames.Activity_Classify, L(PermissionNames.Activity_Classify));
                activityClassifys.CreateChildPermission(PermissionNames.Activity_Classify_Crud, L(PermissionNames.Activity_Classify_Crud));
            }
        }
        
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpActivityConsts.LocalizationSourceName);
        }
    }
}
