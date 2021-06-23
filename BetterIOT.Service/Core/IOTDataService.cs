﻿using BetterIOT.Common;
using BetterIOT.Common.Bus;
using BetterIOT.Service.LocalDB;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class IOTDataService : BackgroundService
    {
        private readonly BusClient bus;
        private readonly ILogger<IOTDataService> logger;

        const string DBFile = "iot.db";
        //LitedbWapper litedb;
        private readonly Timer timer;


        public IOTDataService(ILogger<IOTDataService> logger)
        {
            this.logger = logger;
            bus = new BusClient();
            this.bus.Subscribe(BusOption.DATA_OUTPUT);
            this.bus.OnReceived += Bus_OnReceived;
            //每10分钟执行一次数据清理
            this.timer = new Timer(p => this.ClearData(), null, 0, 1000 * 10 * 60);
        }

        private void ClearData()
        {
            lock (Program.lockdb)
            {
                try
                {

                    using (var db = new LiteDatabase(DBFile))
                    {
                        var col = db.GetCollection<IOTData>();
                        if (col.Count() > 0)
                        {
                            col.EnsureIndex(x => x.Sended);
                            col.DeleteMany(a => a.Sended == true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    logger.LogError(ex.ToString());
                }
            }
        }

        private void Bus_OnReceived(object sender, BusEventArgs e)
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<IOTData>>(e.Message);
            lock (Program.lockdb)
            {
                using (var litedb = new LitedbWapper(DBFile))
                {
                    litedb.Insert<IOTData>(data);
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("DataService started");

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.bus?.Dispose();
        }
    }
}
