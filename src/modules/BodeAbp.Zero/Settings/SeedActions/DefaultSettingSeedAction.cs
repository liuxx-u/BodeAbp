using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework.Default;
using BodeAbp.Zero.Settings.Domain;

namespace BodeAbp.Zero.EntityFramework.SeedActions
{
    public class DefaultSettingSeedAction : IDefaultSeedAction
    {
        /// <summary>
        /// 定义种子数据初始化过程
        /// </summary>
        /// <param name="context"></param>
        public void Action(DbContext context)
        {
            //添加默认配置

        }

        private void AddSettingIfNotExists(DbContext context, string name, string value)
        {
            if (context.Set<Setting>().Any(s => s.Name == name && s.UserId == null))
            {
                return;
            }

            context.Set<Setting>().Add(new Setting(null, name, value));
        }

        /// <summary>
        /// 获取 操作排序，数值越小越先执行
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}
