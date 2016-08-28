using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Abp.Rpc.Coordinate.Zookeeper.Watcher
{
    internal abstract class WatcherBase : org.apache.zookeeper.Watcher
    {
        protected string Path { get; }

        protected WatcherBase(string path)
        {
            Path = path;
        }

        public override async Task process(WatchedEvent watchedEvent)
        {
            if (watchedEvent.getState() != Event.KeeperState.SyncConnected || watchedEvent.getPath() != Path)
                return;
            await ProcessImpl(watchedEvent);
        }

        protected abstract Task ProcessImpl(WatchedEvent watchedEvent);
    }
}
