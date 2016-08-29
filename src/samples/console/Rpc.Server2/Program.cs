using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Abp;
using Abp.Rpc.Address;
using Abp.Rpc.Routing;
using Abp.Rpc.Server;

namespace Rpc.Server2
{
    class Program
    {
        static void Main(string[] args)
        {
            //因为没有引用BodeAbp.Activity中的任何类型
            //所以强制加载BodeAbp.Activity程序集以保证BodeAbp.Activity在AppDomain中被加载。
            Assembly.Load(new AssemblyName("BodeAbp.Product"));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Gray;
            try
            {
                using (var bootstrapper = new AbpBootstrapper())
                {
                    bootstrapper.Initialize();
                    {
                        var serviceEntryManager = bootstrapper.IocManager.Resolve<IServiceEntryManager>();
                        var addressDescriptors = serviceEntryManager.GetEntries().Select(i => new ServiceRoute
                        {
                            Address = new[] { new IpAddress { Ip = "127.0.0.1", Port = 9982 } },
                            ServiceDescriptor = i.Descriptor
                        });

                        var serviceRouteManager = bootstrapper.IocManager.Resolve<IServiceRouteManager>();
                        serviceRouteManager.SetRoutesAsync(addressDescriptors).Wait();
                    }

                    var serviceHost = bootstrapper.IocManager.Resolve<IServiceHost>();

                    Task.Factory.StartNew(async () =>
                    {
                        //启动主机
                        await serviceHost.StartAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9982));
                        Console.WriteLine($"服务端2启动成功，{DateTime.Now}。");
                    });
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
