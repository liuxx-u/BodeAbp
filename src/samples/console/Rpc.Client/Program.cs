using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Rpc.Exceptions;
using Abp.Rpc.ProxyGenerator.Proxy;
using BodeAbp.Activity.Activities;

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
                        var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(IActivitiesAppService) }).ToArray();

                        //创建IUserService的代理。
                        var userService = serviceProxyFactory.CreateProxy<IActivitiesAppService>(services.Single(typeof(IActivitiesAppService).GetTypeInfo().IsAssignableFrom));

                        while (true)
                        {
                            Task.Run(async () =>
                            {
                                try
                                {
                                    Console.WriteLine($"activitiesAppService.GetActivity:{await userService.GetActivity(new IdInput() { Id = 1 })}");
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



