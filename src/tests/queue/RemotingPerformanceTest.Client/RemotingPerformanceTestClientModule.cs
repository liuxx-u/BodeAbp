using Abp.Modules;
using BodeAbp.Queue;
using BodeAbp.Queue.Configuration;
using Castle.Facilities.Logging;

namespace RemotingPerformanceTest.Client
{
    [DependsOn(typeof(BodeAbpQueueModule))]
    public class RemotingPerformanceTestClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            Configuration.Modules.AbpQueue()
                .InitQueue();
        }
    }
}
