using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using WebDemo.WebApi;

namespace WebDemo.Web
{
    [DependsOn(
        typeof(WebDemoCoreModule),
        typeof(WebDemoWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule),
        typeof(AbpWebMvcModule))]
    public class WebDemoWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<WebDemoNavigationProvider>();

            //Configure Hangfire
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
