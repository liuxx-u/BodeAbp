using Abp;
using Abp.Dependency;
using Abp.Modules;
using BodeAbp.Queue.Broker;
using BodeAbp.Queue.Broker.DeleteMessageStrategies;
using BodeAbp.Queue.Broker.Storage;
using BodeAbp.Queue.Clients.Consumers;
using BodeAbp.Queue.Clients.Producers;
using BodeAbp.Queue.Configuration;
using BodeAbp.Queue.Utils;
using Castle.Core.Logging;
using System;

namespace BodeAbp.Queue
{
    [DependsOn(typeof(AbpKernelModule))]
    public class BodeAbpQueueModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IAbpQueueModuleConfiguration, AbpQueueModuleConfiguration>();
        }

        public override void PostInitialize()
        {
            RegisterMissingComponents();
            RegisterUnhandledExceptionHandler();
        }

        private void RegisterMissingComponents()
        {
            IocManager.RegisterIfNot<IDeleteMessageStrategy, DeleteMessageByCountStrategy>();
            IocManager.RegisterIfNot<IAllocateMessageQueueStrategy, AverageAllocateMessageQueueStrategy>();
            IocManager.RegisterIfNot<IQueueSelector, QueueHashSelector>();
            IocManager.RegisterIfNot<IMessageStore, DefaultMessageStore>();
            IocManager.RegisterIfNot<IQueueStore, DefaultQueueStore>();
            IocManager.RegisterIfNot<IConsumeOffsetStore, DefaultConsumeOffsetStore>();
            IocManager.RegisterIfNot<IQueueStore, DefaultQueueStore>();
            IocManager.RegisterIfNot<IChunkStatisticService, DefaultChunkStatisticService>();
            IocManager.Register<IRTStatisticService, DefaultRTStatisticService>(DependencyLifeStyle.Transient);
        }

        private void RegisterUnhandledExceptionHandler()
        {
            var logger = IocManager.Resolve<ILoggerFactory>().Create(GetType().FullName);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => logger.ErrorFormat("Unhandled exception: {0}", e.ExceptionObject);
        }
    }
}
