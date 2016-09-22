using Abp.Modules;
using BodeAbp.Queue.Configuration;
using Castle.Facilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Zero.ProcessorHost
{
    [DependsOn(typeof(BodeAbpZeroModule))]
    public class BodeAbpZeroProcessorHostModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            Configuration.Modules.AbpQueue()
                .InitQueue();
        }
    }
}
