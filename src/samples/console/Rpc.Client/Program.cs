using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Rpc.ProxyGenerator.Proxy;
using BodeAbp.Activity.Activities;
using BodeAbp.Activity.Interactions;
using BodeAbp.Product.Products;

namespace Rpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Gray;
            try
            {
                using (var bootstrapper = new AbpBootstrapper())
                {
                    bootstrapper.Initialize();
                    {
                        var serviceProxyGenerater = bootstrapper.IocManager.Resolve<IServiceProxyGenerater>();
                        var serviceProxyFactory = bootstrapper.IocManager.Resolve<IServiceProxyFactory>();

                        //创建IActivityService的代理。
                        var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(IActivitiesAppService), typeof(IProductsAppService) }).ToArray();
                        var activityService = serviceProxyFactory.CreateProxy<IActivitiesAppService>(services.Single(typeof(IActivitiesAppService).GetTypeInfo().IsAssignableFrom));

                        //创建IProductService的代理。
                        var productService = serviceProxyFactory.CreateProxy<IProductsAppService>(services.Single(typeof(IProductsAppService).GetTypeInfo().IsAssignableFrom));
                        while (true)
                        {
                            Task.Run(async() =>
                            {
                                try
                                {
                                    //Console.WriteLine($"activitiesAppService.GetActivity:{userService.GetType().GetMethods().Last(p => p.Name.Contains("GetActivity")).Invoke(userService, new[] { new IdInput() { Id = 1 } })}");
                                    Console.WriteLine($"activitiesAppService.GetActivity:{await activityService.GetActivity(new IdInput() { Id = 1 })}");

                                    Console.WriteLine($"productAppService.GetProduct:{await productService.GetProduct(1)}");
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }).Wait();
                            Console.ReadLine();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
            }
        }
    }
}



