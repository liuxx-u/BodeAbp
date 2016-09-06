using Abp.Dependency;
using Abp.Schedule;
using Abp.Net.Sockets.Framing;
using Castle.Core.Logging;
using System;

namespace BodeAbp.Queue.Configuration
{
    public class AbpQueueModuleConfiguration : IAbpQueueModuleConfiguration
    {
        public IIocManager IocManager { get; set; }
        public IAbpQueueModuleConfiguration InitQueue()
        {
            //IocManager.Register<IScheduleService, ScheduleService>(DependencyLifeStyle.Transient);
            //IocManager.Register<IMessageFramer, LengthPrefixMessageFramer>(DependencyLifeStyle.Transient);
            return this;

        }

        public IAbpQueueModuleConfiguration RegisterUnhandledExceptionHandler()
        {
            var logger = IocManager.Resolve<ILoggerFactory>().Create(GetType().FullName);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => logger.ErrorFormat("Unhandled exception: {0}", e.ExceptionObject);
            return this;
        }
    }
}
