using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Abp.EntityFramework.Migrations;

namespace Abp.EntityFramework.Default
{
    /// <summary>
    /// 默认数据库种子迁移接口
    /// </summary>
    public interface IDefaultSeedAction : ISeedAction<DefaultDbContext>
    {
    }
}
