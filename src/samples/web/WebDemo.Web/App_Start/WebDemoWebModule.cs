using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using WebDemo.WebApi;
using Abp.Configuration.Startup;
using Abp.AutoMapper;

namespace WebDemo.Web
{
    [DependsOn(
        typeof(WebDemoCoreModule),
        typeof(WebDemoWebApiModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule),
        typeof(AbpWebMvcModule))]
    public class WebDemoWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            
            //Configure Hangfire
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});

            //发送所有错误消息至客户端
            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;

            //不启用EF自动迁移
            Configuration.Modules.AbpEntityFramework().AutoMigrateDatabase = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //压缩输出
            //BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
