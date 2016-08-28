using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Rpc.Coordinate.Zookeeper.Setting
{
    internal class ZoomKeeperSettingNames
    {
        internal const string GroupName = "Abp.Rpc.ZoomKeeper.GroupName";

        internal const string ConnectionString = "Abp.Rpc.ZoomKeeper.ConnectionString";
        internal const string RoutePath = "Abp.Rpc.ZoomKeeper.RoutePath";
        internal const string SessionTimeout = "Abp.Rpc.ZoomKeeper.SessionTimeout";
        internal const string ChRoot = "Abp.Rpc.ZoomKeeper.ChRoot";
    }
}
