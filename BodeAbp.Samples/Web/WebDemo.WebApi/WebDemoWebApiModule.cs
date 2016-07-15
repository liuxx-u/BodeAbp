using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http.Cors;
using BodeAbp.Zero;
using System;
using WebDemo.WebApi.Swagger;
using BodeAbp.Activity;
using BodeAbp.Product;

namespace WebDemo.WebApi
{
    [DependsOn(
        typeof(AbpWebApiModule)
        , typeof(WebDemoCoreModule)
        , typeof(BodeAbpZeroModule)
        , typeof(BodeAbpActivityModule)
        , typeof(BodeAbpProductModule))]
    public class WebDemoWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(WebDemoCoreModule).Assembly, "app")
                .Build();

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(BodeAbpZeroModule).Assembly, "zero")
                .Build();

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(BodeAbpActivityModule).Assembly, "activity")
                .Build();

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(BodeAbpProductModule).Assembly, "product")
                .Build();


            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            var cors = new EnableCorsAttribute("*", "*", "*");
            GlobalConfiguration.Configuration.EnableCors(cors);
            
            ConfigureSwaggerUi();
        }

        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "WebDemo.WebApi");
                    //c.OperationFilter<AuthorizationOperationFilter>();
                    c.DocumentFilter<ApplicationDocumentFilter>();
                    c.IncludeXmlComments(GetXmlCommentsPath(typeof(WebDemoCoreModule)));
                    c.IncludeXmlComments(GetXmlCommentsPath(typeof(BodeAbpZeroModule)));
                    c.IncludeXmlComments(GetXmlCommentsPath(typeof(BodeAbpActivityModule)));
                    c.IncludeXmlComments(GetXmlCommentsPath(typeof(BodeAbpProductModule)));
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi(c => {
                    c.CustomAsset("index", typeof(WebDemoWebApiModule).Assembly, "WebDemo.WebApi.Swagger.index.html");
                    c.InjectStylesheet(typeof(WebDemoWebApiModule).Assembly, "WebDemo.WebApi.Swagger.theme-flattop.css");
                    c.InjectJavaScript(typeof(WebDemoWebApiModule).Assembly, "WebDemo.WebApi.Swagger.translator.js");
                });
        }

        private static string GetXmlCommentsPath(Type moduleType)
        {
            return string.Format(@"{0}\bin\{1}.XML", AppDomain.CurrentDomain.BaseDirectory, moduleType.Assembly.GetName().Name);
        }
    }
}
