using Abp.Modules;
using BodeAbp.Queue;
using BodeAbp.Queue.Configuration;
using Castle.Facilities.Logging;

namespace Queue.Broker
{
    [DependsOn(typeof(BodeAbpQueueModule))]
    public class QueueBrokerModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            Configuration.Modules.AbpQueue()
                .InitQueue()
                .UseDeleteMessageByCountStrategy(10);
        }
    }
}
