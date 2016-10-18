using Abp;
using BodeAbp.Queue.Broker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = new AbpBootstrapper())
            {
                bootstrapper.Initialize();

                var setting = new BrokerSetting(
                bool.Parse(ConfigurationManager.AppSettings["isMemoryMode"]),
                ConfigurationManager.AppSettings["fileStoreRootPath"],
                chunkCacheMaxPercent: 95,
                chunkFlushInterval: int.Parse(ConfigurationManager.AppSettings["flushInterval"]),
                messageChunkDataSize: int.Parse(ConfigurationManager.AppSettings["chunkSize"]) * 1024 * 1024,
                chunkWriteBuffer: int.Parse(ConfigurationManager.AppSettings["chunkWriteBuffer"]) * 1024,
                enableCache: bool.Parse(ConfigurationManager.AppSettings["enableCache"]),
                chunkCacheMinPercent: int.Parse(ConfigurationManager.AppSettings["chunkCacheMinPercent"]),
                syncFlush: bool.Parse(ConfigurationManager.AppSettings["syncFlush"]),
                messageChunkLocalCacheSize: 30 * 10000,
                queueChunkLocalCacheSize: 10000)
                {
                    NotifyWhenMessageArrived = bool.Parse(ConfigurationManager.AppSettings["notifyWhenMessageArrived"]),
                    MessageWriteQueueThreshold = int.Parse(ConfigurationManager.AppSettings["messageWriteQueueThreshold"])
                };
                BrokerController.Create(setting).Start();
                Console.ReadLine();
            }
        }
    }
}
