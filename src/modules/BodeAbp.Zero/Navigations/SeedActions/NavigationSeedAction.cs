using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework.Default;
using BodeAbp.Zero.Navigations.Domain;

namespace BodeAbp.Zero.Navigations.SeedActions
{
    /// <summary>
    /// 导航  种子数据
    /// </summary>
    public class NavigationSeedAction : IDefaultSeedAction
    {
        public int Order
        {
            get { return 1; }
        }

        public void Action(DbContext context)
        {
            if (!context.Set<Navigation>().Any())
            {
                context.Set<Navigation>().Add(new Navigation
                {
                    
                });
            }
        }
    }
}
