using Abp;
using Abp.Application.Services;
using Abp.Net.Sockets;
using BodeAbp.Queue.Application.RequestHandlers;
using BodeAbp.Queue.Clients.Consumers;
using BodeAbp.Queue.Protocols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;

namespace BodeAbp.Zero.ProcessorHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = new AbpBootstrapper())
            {
                bootstrapper.Initialize();

                var address = ConfigurationManager.AppSettings["BrokerAddress"];
                var brokerAddress = string.IsNullOrEmpty(address) ? SocketUtils.GetLocalIPV4() : IPAddress.Parse(address);
                var clientCount = int.Parse(ConfigurationManager.AppSettings["ClientCount"]);
                var setting = new ConsumerSetting
                {
                    ConsumeFromWhere = ConsumeFromWhere.FirstOffset,
                    MessageHandleMode = MessageHandleMode.Parallel,
                    BrokerAddress = new IPEndPoint(brokerAddress, 5001),
                    BrokerAdminAddress = new IPEndPoint(brokerAddress, 5002)
                };
                var messageHandler = new ApplicationServiceInvokeRequestHandler();
                for (var i = 1; i <= clientCount; i++)
                {
                    new Consumer(ConfigurationManager.AppSettings["ConsumerGroup"], setting)
                        .Subscribe("BodeAbp.Zero")
                        .SetMessageHandler(messageHandler)
                        .Start();
                }
                Console.ReadLine();
            }
        }
    }
}
