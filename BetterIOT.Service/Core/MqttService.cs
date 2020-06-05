using BetterIOT.Common;
using BetterIOT.Common.Bus;
using BetterIOT.Service.Base;
using BetterIOT.Service.LocalDB;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class MqttService : BackgroundService
    {
        private readonly ILogger<MqttService> logger;
        MQTTTranData mQTTTran;
        private readonly BusClient bus;
        const string DBFile = "iot.db";
        /// <summary>
        /// 线程取消
        /// </summary>
        CancellationToken _CancellationToken;
        public MqttService(ILogger<MqttService> logger)
        {
            this.logger = logger;
            mQTTTran = new MQTTTranData(Program.configInfo.ClientId, Program.configInfo.MQTTerver, Program.configInfo.MQTTPort, Program.configInfo.MQTTUsr, Program.configInfo.MQTTPsw);
            mQTTTran.OnGetData += MQTTTran_OnGetData;
            mQTTTran.SubTopic(new string[] { "" });
            mQTTTran.Connect();
            this.bus = new BusClient();
            this.bus.Subscribe(BusOption.CONFIG_CHANGE);
            this.bus.OnReceived += Bus_OnReceived;
        }
        private void Bus_OnReceived(object sender, BusEventArgs e)
        {
            
        }
        private static void MQTTTran_OnGetData(object sender, string ClientId, string topic, string e)
        {

        }

        private void SendData()
        {
            while (!_CancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var db = new LiteDatabase(DBFile))
                    {
                        var col = db.GetCollection<IOTData>(typeof(IOTData).ToString());
                        IEnumerable<IOTData> iOTs = col.Find(x => x.Sended == false);
                        bool sendok = mQTTTran.SendMessageTB(System.Text.Json.JsonSerializer.Serialize(iOTs), Program.configInfo.Topic_IOTDATA);
                        if (sendok == true)
                        {
                            //发送成功,改变数据状态
                            foreach (IOTData iot in iOTs)
                            {
                                iot.Sended = true;
                                col.Update(iot);
                            }
                        }
                        else
                        {
                            //发送失败
                            mQTTTran.Connect();
                        }
                    }
                }
                catch(Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _CancellationToken = stoppingToken;
            return Task.Factory.StartNew(SendData);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.bus?.Dispose();
            mQTTTran.Disconnect();
        }
    }
}
