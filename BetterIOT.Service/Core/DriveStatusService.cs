using BetterIOT.Common;
using BetterIOT.Common.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class DriveStatusService : BackgroundService
    {
        private readonly BusClient bus;
        private readonly ILogger<DriveStatusService> logger;

        public DriveStatusService(ILogger<DriveStatusService> logger)
        {
            this.logger = logger;
            bus = new BusClient();
            this.bus.Subscribe(BusOption.DRIVE_HEARTBEAT);
            this.bus.OnReceived += Bus_OnReceived;
        }

        private void Bus_OnReceived(object sender, BusEventArgs e)
        {
            DriveHeartBeat driveHeart = System.Text.Json.JsonSerializer.Deserialize<DriveHeartBeat>(e.Message);
            if (Program.DriveOnlineInfo.ContainsKey(driveHeart.DriveCode) == true)
            {
                //存在
                Program.DriveOnlineInfo[driveHeart.DriveCode] = driveHeart.hTime;
            }
            else
            {
                //不存在
                Program.DriveOnlineInfo.Add(driveHeart.DriveCode, driveHeart.hTime);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("DriveStatusService started");

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.bus?.Dispose();
        }
    }
}
