using org.apache.zookeeper;
using Abp.Rpc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Helper;
using Abp.Rpc.Routing.Args;
using Abp.Configuration;
using Abp.Rpc.Coordinate.Zookeeper.Watcher;

namespace Abp.Rpc.Coordinate.Zookeeper
{
    /// <summary>
    /// 基于zookeeper的服务路由管理者。
    /// </summary>
    public class ZooKeeperServiceRouteManager : ServiceRouteManagerBase, IDisposable
    {
        #region Field

        private ZooKeeper _zooKeeper;
        private readonly ZookeeperConfigInfo _configInfo;
        private readonly IServiceRouteFactory _serviceRouteFactory;
        private ServiceRoute[] _routes;
        private readonly ManualResetEvent _connectionWait = new ManualResetEvent(false);

        #endregion Field

        #region Constructor

        public ZooKeeperServiceRouteManager(SettingManager settingManager, IServiceRouteFactory serviceRouteFactory)
        {
            _configInfo = new ZookeeperConfigInfo(settingManager);
            _serviceRouteFactory = serviceRouteFactory;
            CreateZooKeeper().Wait();
            EnterRoutes().Wait();
        }

        #endregion Constructor

        #region Overrides of ServiceRouteManagerBase

        /// <summary>
        /// 获取所有可用的服务路由信息。
        /// </summary>
        /// <returns>服务路由集合。</returns>
        public override async Task<IEnumerable<ServiceRoute>> GetRoutesAsync()
        {
            await EnterRoutes();
            return _routes;
        }

        /// <summary>
        /// 清空所有的服务路由。
        /// </summary>
        /// <returns>一个任务。</returns>
        public override async Task ClearAsync()
        {
            //准备清空所有路由配置
            var path = _configInfo.RoutePath;
            var childrens = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var index = 0;
            while (childrens.Any())
            {
                var nodePath = "/" + string.Join("/", childrens);

                if (await _zooKeeper.existsAsync(nodePath) != null)
                {
                    var result = await _zooKeeper.getChildrenAsync(nodePath);
                    if (result?.Children != null)
                    {
                        foreach (var child in result.Children)
                        {
                            var childPath = $"{nodePath}/{child}";
                            //$"准备删除：{childPath}。"
                            await _zooKeeper.deleteAsync(childPath);
                        }
                    }
                    //$"准备删除：{nodePath}。"
                    await _zooKeeper.deleteAsync(nodePath);
                }
                index++;
                childrens = childrens.Take(childrens.Length - index).ToArray();
            }
            //路由配置清空完成
        }

        /// <summary>
        /// 设置服务路由。
        /// </summary>
        /// <param name="routes">服务路由集合。</param>
        /// <returns>一个任务。</returns>
        protected override async Task SetRoutesAsync(IEnumerable<ServiceRouteDescriptor> routes)
        {
            //准备添加服务路由
            await CreateSubdirectory(_configInfo.RoutePath);

            var path = _configInfo.RoutePath;
            if (!path.EndsWith("/"))
                path += "/";

            routes = routes.ToArray();

            if (_routes != null)
            {
                var oldRouteIds = _routes.Select(i => i.ServiceDescriptor.Id).ToArray();
                var newRouteIds = routes.Select(i => i.ServiceDescriptor.Id).ToArray();
                var deletedRouteIds = oldRouteIds.Except(newRouteIds).ToArray();
                foreach (var deletedRouteId in deletedRouteIds)
                {
                    var nodePath = $"{path}{deletedRouteId}";
                    await _zooKeeper.deleteAsync(nodePath);
                }
            }

            foreach (var serviceRoute in routes)
            {
                var nodePath = $"{path}{serviceRoute.ServiceDescriptor.Id}";
                var nodeData = SerializeHelper.ToBinary(serviceRoute);
                if (await _zooKeeper.existsAsync(nodePath) == null)
                {
                    //$"节点：{nodePath}不存在将进行创建。"
                    await _zooKeeper.createAsync(nodePath, nodeData, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
                }
                else
                {
                    //$"将更新节点：{nodePath}的数据。"
                    var onlineData = (await _zooKeeper.getDataAsync(nodePath)).Data;
                    if (!DataEquals(nodeData, onlineData))
                        await _zooKeeper.setDataAsync(nodePath, nodeData);
                }
            }
            //服务路由添加成功
        }

        #endregion Overrides of ServiceRouteManagerBase

        #region Private Method

        private async Task CreateZooKeeper()
        {
            if (_zooKeeper != null)
                await _zooKeeper.closeAsync();
            _zooKeeper = new ZooKeeper(_configInfo.ConnectionString, (int)_configInfo.SessionTimeout.TotalMilliseconds
                , new ReconnectionWatcher(
                    () =>
                    {
                        _connectionWait.Set();
                    },
                    async () =>
                    {
                        _connectionWait.Reset();
                        await CreateZooKeeper();
                    }));
        }

        private async Task CreateSubdirectory(string path)
        {
            _connectionWait.WaitOne();
            if (await _zooKeeper.existsAsync(path) != null)
                return;

            //$"节点{path}不存在，将进行创建。"
            var childrens = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var nodePath = "/";

            foreach (var children in childrens)
            {
                nodePath += children;
                if (await _zooKeeper.existsAsync(nodePath) == null)
                {
                    await _zooKeeper.createAsync(nodePath, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
                }
                nodePath += "/";
            }
        }

        private async Task<ServiceRoute> GetRoute(byte[] data)
        {
            //$"准备转换服务路由。"
            if (data == null)return null;

            var descriptor = SerializeHelper.FromBinary<ServiceRouteDescriptor>(data);

            return (await _serviceRouteFactory.CreateServiceRoutesAsync(new[] { descriptor })).First();
        }

        private async Task<ServiceRoute> GetRoute(string path)
        {
            var watcher = new NodeMonitorWatcher(_zooKeeper, path,
                async (oldData, newData) => await NodeChange(oldData, newData));
            var data = (await _zooKeeper.getDataAsync(path, watcher)).Data;
            watcher.SetCurrentData(data);
            return await GetRoute(data);
        }

        private async Task<ServiceRoute[]> GetRoutes(IEnumerable<string> childrens)
        {
            var rootPath = _configInfo.RoutePath;
            if (!rootPath.EndsWith("/"))
                rootPath += "/";

            childrens = childrens.ToArray();
            var routes = new List<ServiceRoute>(childrens.Count());

            foreach (var children in childrens)
            {
                //$"准备从节点：{children}中获取路由信息。"
                var nodePath = $"{rootPath}{children}";
                routes.Add(await GetRoute(nodePath));
            }

            return routes.ToArray();
        }

        private async Task EnterRoutes()
        {
            if (_routes != null)
                return;
            _connectionWait.WaitOne();

            var watcher = new ChildrenMonitorWatcher(_zooKeeper, _configInfo.RoutePath,
                async (oldChildrens, newChildrens) => await ChildrenChange(oldChildrens, newChildrens));
            if (await _zooKeeper.existsAsync(_configInfo.RoutePath, watcher) != null)
            {
                var result = await _zooKeeper.getChildrenAsync(_configInfo.RoutePath, watcher);
                var childrens = result.Children.ToArray();
                watcher.SetCurrentData(childrens);
                _routes = await GetRoutes(childrens);
            }
            else
            {
                //$"无法获取路由信息，因为节点：{_configInfo.RoutePath}，不存在。"
                _routes = new ServiceRoute[0];
            }
        }

        private static bool DataEquals(IReadOnlyList<byte> data1, IReadOnlyList<byte> data2)
        {
            if (data1.Count != data2.Count)
                return false;
            for (var i = 0; i < data1.Count; i++)
            {
                var b1 = data1[i];
                var b2 = data2[i];
                if (b1 != b2)
                    return false;
            }
            return true;
        }

        public async Task NodeChange(byte[] oldData, byte[] newData)
        {
            if (DataEquals(oldData, newData))
                return;

            var newRoute = await GetRoute(newData);
            //得到旧的路由。
            var oldRoute = _routes.First(i => i.ServiceDescriptor.Id == newRoute.ServiceDescriptor.Id);

            lock (_routes)
            {
                //删除旧路由，并添加上新的路由。
                _routes =
                    _routes
                        .Where(i => i.ServiceDescriptor.Id != newRoute.ServiceDescriptor.Id)
                        .Concat(new[] { newRoute }).ToArray();
            }
            //触发路由变更事件。
            OnChanged(new ServiceRouteChangedEventArgs(newRoute, oldRoute));
        }

        public async Task ChildrenChange(string[] oldChildrens, string[] newChildrens)
        {
            //$"最新的节点信息：{string.Join(",", newChildrens)}"

            //$"旧的节点信息：{string.Join(",", oldChildrens)}"

            //计算出已被删除的节点。
            var deletedChildrens = oldChildrens.Except(newChildrens).ToArray();
            //计算出新增的节点。
            var createdChildrens = newChildrens.Except(oldChildrens).ToArray();

            //$"需要被删除的路由节点：{string.Join(",", deletedChildrens)}"
            //$"需要被添加的路由节点：{string.Join(",", createdChildrens)}"

            //获取新增的路由信息。
            var newRoutes = (await GetRoutes(createdChildrens)).ToArray();

            var routes = _routes.ToArray();
            lock (_routes)
            {
                _routes = _routes
                    //删除无效的节点路由。
                    .Where(i => !deletedChildrens.Contains(i.ServiceDescriptor.Id))
                    //连接上新的路由。
                    .Concat(newRoutes)
                    .ToArray();
            }
            //需要删除的路由集合。
            var deletedRoutes = routes.Where(i => deletedChildrens.Contains(i.ServiceDescriptor.Id)).ToArray();
            //触发删除事件。
            OnRemoved(deletedRoutes.Select(route => new ServiceRouteEventArgs(route)).ToArray());

            //触发路由被创建事件。
            OnCreated(newRoutes.Select(route => new ServiceRouteEventArgs(route)).ToArray());

            //"路由数据更新成功。"
        }

        #endregion Private Method

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _connectionWait.Dispose();
            _zooKeeper.closeAsync().Wait();
        }

        #endregion Implementation of IDisposable
    }
}
