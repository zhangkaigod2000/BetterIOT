using BetterIOT.Common.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class BusService : BackgroundService
    {
        private readonly ILogger<BusService> logger;
        private readonly XSubscriberSocket xsubSocket;
        private readonly XPublisherSocket xpubSocket;

        public BusService(ILogger<BusService> logger)
        {
            this.logger = logger;
            this.xsubSocket = new XSubscriberSocket();
            this.xpubSocket = new XPublisherSocket();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            xpubSocket.Bind(BusOption.SubscriberAddress);
            xsubSocket.Bind(BusOption.PublisherAddress);

            this.logger.LogInformation("MQBusService started");

            var proxy = new Proxy(xsubSocket, xpubSocket);
            return Task.Run(proxy.Start);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.xsubSocket?.Dispose();
            this.xpubSocket?.Dispose();
        }
    }
}
