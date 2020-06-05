using BetterIOT.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BetterIOT.Service
{
    class Program
    {
        /// <summary>
        /// 设备驱动在线情况
        /// </summary>
        public static Dictionary<string, DateTime> DriveOnlineInfo = new Dictionary<string, DateTime>();
        static async Task  Main(string[] args)
        {
            var builder = new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               ///启动UDP网络发现服务
               services.AddHostedService<Udp_Discovery>();

               ///启动MQBus服务
               services.AddHostedService<BusService>();


               // ///启动Mqtt服务
               // services.AddHostedService<MqttService>();

               ///启动驱动运行状态服务
               services.AddHostedService<DriveStatusService>();

               ///启动数据库服务
               services.AddHostedService<IOTDataService>();
           })
           .ConfigureLogging((hostContext, logging) =>
           {
               logging.AddConsole();
           });

            await builder.RunConsoleAsync();
        }
    }
}
