using System;
using Abp.Configuration;
using Abp.Rpc.Coordinate.Zookeeper.Setting;

namespace Abp.Rpc.Coordinate.Zookeeper
{
    /// <summary>
    /// zookeeper配置信息。
    /// </summary>
    internal class ZookeeperConfigInfo
    {
        /// <summary>
        /// 初始化一个会话超时为20秒的Zookeeper连接信息。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="routePath">路由配置路径。</param>
        /// <param name="chRoot">根节点。</param>
        public ZookeeperConfigInfo(string connectionString, string routePath = "/dotnet/serviceRoutes", string chRoot = null) : this(connectionString, TimeSpan.FromSeconds(20), routePath, chRoot)
        {
        }

        /// <summary>
        /// 初始化一个新的Zookeeper连接信息。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="routePath">路由配置路径。</param>
        /// <param name="sessionTimeout">会话超时时间。</param>
        /// <param name="chRoot">根节点。</param>
        public ZookeeperConfigInfo(string connectionString, TimeSpan sessionTimeout, string routePath = "/dotnet/serviceRoutes", string chRoot = null)
        {
            ChRoot = chRoot;
            ConnectionString = connectionString;
            RoutePath = routePath;
            SessionTimeout = sessionTimeout;
        }

        public ZookeeperConfigInfo(SettingManager settingManager)
        {
            ConnectionString = settingManager.GetSettingValue(ZoomKeeperSettingNames.ConnectionString);
            RoutePath = settingManager.GetSettingValue(ZoomKeeperSettingNames.RoutePath);
            ConnectionString = settingManager.GetSettingValue(ZoomKeeperSettingNames.ConnectionString);
            ChRoot = settingManager.GetSettingValue(ZoomKeeperSettingNames.ChRoot);

            double timeout;
            if (double.TryParse(settingManager.GetSettingValue(ZoomKeeperSettingNames.SessionTimeout), out timeout))
            {
                SessionTimeout = TimeSpan.FromSeconds(timeout);
            }
            else
            {
                SessionTimeout = TimeSpan.FromSeconds(20);
            }
        }

        /// <summary>
        /// 连接字符串。
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 路由配置路径。
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// 会话超时时间。
        /// </summary>
        public TimeSpan SessionTimeout { get; set; }

        /// <summary>
        /// 根节点。
        /// </summary>
        public string ChRoot { get; set; }
    }
}
