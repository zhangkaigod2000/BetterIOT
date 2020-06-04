using BetterIOT.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BetterIOT.Service
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            var builder = new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<Udp_Discovery>();

               // ///启动MQBus服务
               services.AddHostedService<BusService>();

               // ///启动Mqtt服务
               // services.AddHostedService<MqttService>();

               // ///启动Udp服务
               // services.AddHostedService<UdpService>();

               // ///启动数据库服务
               // services.AddHostedService<DataService>();
           })
           .ConfigureLogging((hostContext, logging) =>
           {
               logging.AddConsole();
           });

            await builder.RunConsoleAsync();
        }
    }
}
