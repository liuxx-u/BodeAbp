using System;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Abp.Rpc.Coordinate.Zookeeper.Watcher
{
    internal class ReconnectionWatcher : org.apache.zookeeper.Watcher
    {
        private readonly Action _connectioned;
        private readonly Action _disconnect;

        public ReconnectionWatcher(Action connectioned, Action disconnect)
        {
            _connectioned = connectioned;
            _disconnect = disconnect;
        }

        #region Overrides of Watcher

        /// <summary>Processes the specified event.</summary>
        /// <param name="watchedEvent">The event.</param>
        /// <returns></returns>
        public override async Task process(WatchedEvent watchedEvent)
        {
            if (watchedEvent.getState() == Event.KeeperState.SyncConnected)
            {
                _connectioned();
            }
            else
            {
                _disconnect();
            }
                await Task.FromResult(1);
        }

        #endregion Overrides of Watcher
    }
}
