using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework.Default;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Users.Domain;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Users.SeedActions
{
    /// <summary>
    /// 用户种子数据
    /// </summary>
    public class UserSeedAction : IDefaultSeedAction
    {
        /// <summary>
        /// 迁移时执行方法
        /// </summary>
        /// <param name="context"></param>
        public void Action(DbContext context)
        {
            //Admin user
            var adminUser = context.Set<User>().FirstOrDefault(r => r.Name == BodeAbpZeroConsts.StaticUserName);
            if (adminUser == null)
            {
                adminUser = context.Set<User>().Add(new User
                {
                    UserName = BodeAbpZeroConsts.StaticUserName,
                    Password = new PasswordHasher().HashPassword(BodeAbpZeroConsts.StaticUserPassword),
                    IsActive = true
                });
            }
        }

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Order
        {
            get
            {
                return 3;
            }
        }
    }
}
