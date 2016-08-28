using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Rpc.Coordinate.Zookeeper.Setting;

namespace Abp.Rpc.Coordinate.Zookeeper
{
    [DependsOn(typeof(AbpRpcModule))]
    public class AbpRpcCoordinateZookeeperModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Settings.Providers.Add<ZoomKeeperSettingProvider>();
        }
    }
}
