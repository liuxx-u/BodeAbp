using System.Data.Entity;
using System.Linq;
using Abp.Authorization;
using Abp.EntityFramework.Default;
using BodeAbp.Zero.Permissions.Domain;
using BodeAbp.Zero.Roles.Domain;

namespace BodeAbp.Zero.Roles.SeedActions
{
    public class HostRoleSeedAction : IDefaultSeedAction
    {
        /// <summary>
        /// 定义种子数据初始化过程
        /// </summary>
        /// <param name="context"></param>
        public void Action(DbContext context)
        {
            //Admin role for host
            var adminRoleForHost = context.Set<Role>().FirstOrDefault(r => r.Name == BodeAbpZeroConsts.StaticRoleName);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = context.Set<Role>().Add(new Role { Name = BodeAbpZeroConsts.StaticRoleName, DisplayName = BodeAbpZeroConsts.StaticRoleName, IsStatic = true });
                
            }
        }

        /// <summary>
        /// 获取 操作排序，数值越小越先执行
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
