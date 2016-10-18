﻿using System.Reflection;
using Abp.Modules;

namespace Abp.TestBase
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpTestBaseModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}