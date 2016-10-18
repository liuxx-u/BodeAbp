using Abp.Authorization;
using Abp.Localization;

namespace BodeAbp.Zero.Providers
{
    public class BodeAbpZeroAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var zero = context.GetPermissionOrNull(PermissionNames.Zero);
            if (zero == null)
            {
                zero = context.CreatePermission(PermissionNames.Zero, L(PermissionNames.Zero));


                var zeroRoles = zero.CreateChildPermission(PermissionNames.Zero_Role, L(PermissionNames.Zero_Role));
                zeroRoles.CreateChildPermission(PermissionNames.Zero_Role_Crud, L(PermissionNames.Zero_Role_Crud));
                zeroRoles.CreateChildPermission(PermissionNames.Zero_Role_Test, L(PermissionNames.Zero_Role_Test));

                var zeroUsers = zero.CreateChildPermission(PermissionNames.Zero_User, L(PermissionNames.Zero_User));
                zeroUsers.CreateChildPermission(PermissionNames.Zero_User_Crud, L(PermissionNames.Zero_User_Crud));


                //审计日志
                var zeroAuditLogs = zero.CreateChildPermission(PermissionNames.Zero_AuditLog, L(PermissionNames.Zero_AuditLog));
                zeroAuditLogs.CreateChildPermission(PermissionNames.Zero_AuditLog_Crud, L(PermissionNames.Zero_AuditLog_Crud));
            }
        }
        
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpZeroConsts.LocalizationSourceName);
        }
    }
}
