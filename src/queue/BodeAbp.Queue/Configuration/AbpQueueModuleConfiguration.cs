using Abp.Dependency;
using BodeAbp.Queue.Broker.Client;
using BodeAbp.Queue.Broker.LongPolling;
using Castle.MicroKernel.Registration;
using BodeAbp.Queue.Broker.DeleteMessageStrategies;
using BodeAbp.Queue.Clients.Producers;

namespace BodeAbp.Queue.Configuration
{
    public class AbpQueueModuleConfiguration : IAbpQueueModuleConfiguration
    {
        public IIocManager IocManager { get; set; }
        public IAbpQueueModuleConfiguration InitQueue()
        {
            IocManager.Register<ProducerManager, ProducerManager>();
            IocManager.Register<ConsumerManager, ConsumerManager>();
            IocManager.Register<SuspendedPullRequestManager, SuspendedPullRequestManager>();
            return this;
        }

        public IAbpQueueModuleConfiguration UseDeleteMessageByTimeStrategy(int maxStorageHours = 24 * 30)
        {
            IocManager.IocContainer.Register(Component.For<IDeleteMessageStrategy>().UsingFactoryMethod(() => new DeleteMessageByTimeStrategy(maxStorageHours)));
            return this;
        }
        public IAbpQueueModuleConfiguration UseDeleteMessageByCountStrategy(int maxChunkCount = 100)
        {
            IocManager.IocContainer.Register(Component.For<IDeleteMessageStrategy>().UsingFactoryMethod(() => new DeleteMessageByCountStrategy(maxChunkCount)));
            return this;
        }
        public IAbpQueueModuleConfiguration UseQueueHashSelector()
        {
            IocManager.Register<IQueueSelector, QueueHashSelector>();
            return this;
        }
        public IAbpQueueModuleConfiguration UseQueueAverageSelector()
        {
            IocManager.Register<IQueueSelector, QueueAverageSelector>();
            return this;
        }
    }
}
