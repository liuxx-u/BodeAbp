using Abp.Authorization;
using WebDemo.Authorization.Roles;
using WebDemo.MultiTenancy;
using WebDemo.Authorization.Users;

namespace WebDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
