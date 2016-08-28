using System;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Abp.Rpc.Coordinate.Zookeeper.Watcher
{
    internal class NodeMonitorWatcher : WatcherBase
    {
        private readonly ZooKeeper _zooKeeper;
        private readonly Action<byte[], byte[]> _action;
        private byte[] _currentData;

        public NodeMonitorWatcher(ZooKeeper zooKeeper, string path, Action<byte[], byte[]> action) : base(path)
        {
            _zooKeeper = zooKeeper;
            _action = action;
        }

        public NodeMonitorWatcher SetCurrentData(byte[] currentData)
        {
            _currentData = currentData;

            return this;
        }

        #region Overrides of WatcherBase

        protected override async Task ProcessImpl(WatchedEvent watchedEvent)
        {
            var path = Path;
            switch (watchedEvent.get_Type())
            {
                case Event.EventType.NodeDataChanged:
                    var watcher = new NodeMonitorWatcher(_zooKeeper, path, _action);
                    var data = await _zooKeeper.getDataAsync(path, watcher);
                    var newData = data.Data;
                    _action(_currentData, newData);
                    watcher.SetCurrentData(newData);
                    break;

                    /*case Event.EventType.NodeDeleted:
                            _action(_currentData, null);
                            break;*/
            }
        }

        #endregion Overrides of WatcherBase
    }
}
